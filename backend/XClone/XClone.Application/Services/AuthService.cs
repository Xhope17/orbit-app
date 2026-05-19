using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Helpers;
using XClone.Application.Models.Requets.Auth;
using XClone.Application.Models.Requets.Auth.RecoverPassword;
using XClone.Application.Models.Requets.Auth.Register;
using XClone.Application.Models.Requets.User;
using XClone.Application.Models.Responses;
using XClone.Application.Models.Responses.Auth;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Exceptions;
using XClone.Shared;
using XClone.Shared.Constants;
using XClone.Shared.Helpers;

namespace XClone.Application.Services
{
    //public class AuthService(IUserRepository userRepository, IConfiguration configuration, ICacheService cacheService) : IAuthService
    public class AuthService(IUnitOfWork uow, IConfiguration configuration, ICacheService cacheService,
        IEmailTemplateService emailTemplateService, SMTP smtp, IUserService userService) : IAuthService
    {

        public async Task<GenericResponse<LoginAuthResponse>> Login(LoginAuthRequest model)
        {
            var user = await uow.userRepository.Get(model.Email)
                ?? throw new BadRequestException(ResponseConstants.AUTH_USER_OR_PASSWORD_NOT_FOUND);

            var validatePassword = Hasher.ComparePassword(model.Password, user.Password);
            if (!validatePassword)
            {
                var templateFailed = await emailTemplateService.Get(EmailTemplateNameConstants.AUTH_LOGIN_FAILED, []);
                await smtp.Send(model.Email, templateFailed.Subject, templateFailed.Body);
                throw new BadRequestException(ResponseConstants.AUTH_USER_OR_PASSWORD_NOT_FOUND);
            }

            //var token = TokenHelper.Create(user.Id, configuration, cacheService);
            var token = TokenHelper.Create(user.Id, [.. user.UserRoleUsers.Select(x => x.Role.Name)], configuration, cacheService);
            var refreshToken = TokenHelper.CreateRefresh(user.Id, configuration, cacheService);

            var templateSuccess = await emailTemplateService.Get(EmailTemplateNameConstants.AUTH_LOGIN_SUCCESS, new Dictionary<string, string>
            {
                { "datetime", DateTimeHelper.UtcNow().ToString() }
            });
            await smtp.Send(model.Email, templateSuccess.Subject, templateSuccess.Body);

            return ResponseHelper.Create(new LoginAuthResponse
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        //Refrescar el token de autenticación, con un token de refresco válido
        public async Task<GenericResponse<LoginAuthResponse>> Renew(RenewAuthRequest model)
        {
            var findRefreshToken = cacheService.Get<RefreshToken>(CacheHelper.AuthRefreshTokenKey(model.RefreshToken))
                ?? throw new NotFoundException(ResponseConstants.AUTH_REFRESH_TOKEN_NOT_FOUND);

            //var token = TokenHelper.Create(findRefreshToken.UserId, configuration, cacheService);
            var user = await uow.userRepository.Get(findRefreshToken.UserId)
                ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST);

            var token = TokenHelper.Create(findRefreshToken.UserId, [.. user.UserRoleUsers.Select(x => x.Role.Name)], configuration, cacheService);
            var refreshToken = TokenHelper.CreateRefresh(findRefreshToken.UserId, configuration, cacheService);

            cacheService.Delete(CacheHelper.AuthRefreshTokenKey(model.RefreshToken));

            return ResponseHelper.Create(new LoginAuthResponse
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        //metodo para iniciar el proceso de registro, enviando un correo electrónico con un enlace de verificación,
        //que contiene un token temporal, para validar el correo electrónico del usuario
        public async Task<GenericResponse<string>> RegisterInit(RegisterInitAuthRequest model)
        {
            if (await uow.userRepository.IfExists((x => x.Email == model.Email)))
            {
                throw new BadRequestException("El correo electrónico que ingresó, está registrado en la plataforma");
            }

            var token = Generate.RandomText();
            var cacheKey = CacheHelper.AuthRegisterTokenCreation(token, TimeSpan.FromMinutes(5));

            var url = $"{configuration[ConfigurationConstants.CLIENT_ORIGIN]}/register/{token}";
            var template = await emailTemplateService.Get(EmailTemplateNameConstants.AUTH_REGISTER_EMAIL_VERIFICATION, new Dictionary<string, string>
            {
                { "url", url }
            });

            await smtp.Send(model.Email, template.Subject, template.Body);
            cacheService.Create(cacheKey.Key, cacheKey.Expiration, new RegisterInitAuthResponse
            {
                Email = model.Email
            });

            return ResponseHelper.Create("Enlace para verificación envíado correctamente");
        }


        //metodo para validar el token temporal enviado al correo electrónico del usuario, para verificar su correo electrónico, y permitirle completar su registro
        public async Task<GenericResponse<RegisterInitAuthResponse>> RegisterValidateToken(string token)
        {
            var findToken = cacheService.Get<RegisterInitAuthResponse>(CacheHelper.AuthRegisterTokenKey(token));

            return findToken is null
                ? throw new NotFoundException("El token no existe o expiró")
                : ResponseHelper.Create(findToken);
        }



        public async Task<GenericResponse<UserDto>> RegisterComplete(CreateUserRequest model, string token)
        {
            var findToken = cacheService.Get<RegisterInitAuthResponse>(CacheHelper.AuthRegisterTokenKey(token));
            if (findToken is null)
            {
                throw new NotFoundException("El token no existe o expiró");
            }

            if (findToken.Email != model.Email)
            {
                throw new BadRequestException("El correo electrónico debe ser el mismo, que se uso al comenzar el proceso de registro.");
            }

            return await userService.Create(model, null);
        }

        public async Task<GenericResponse<string>> RecoverPasswordSendOTP(RecoverPasswordSendOTPAuthRequest model)
        {
            var user = await uow.userRepository.Get(x => x.Email == model.Email);

            if (user is not null)
            {
                var otp = Generate.RandomText(length: 8);
                var cacheKey = CacheHelper.AuthRecoverPasswordOTPCreation(otp, TimeSpan.FromMinutes(3));

                var template = await emailTemplateService.Get(EmailTemplateNameConstants.AUTH_RECOVER_PASSWORD_OTP, new Dictionary<string, string>
                {
                    { "otp", otp }
                });
                await smtp.Send(model.Email, template.Subject, template.Body);

                cacheService.Create(cacheKey.Key, cacheKey.Expiration, user.Id);
            }

            return ResponseHelper.Create("Si su correo electrónico existe, recibirá un correo electrónico a su cuenta, con un código que le permitirá realizar su cambio.");
        }

        public async Task<GenericResponse<string>> RecoverPassword(RecoverPasswordAuthRequest model, string code)
        {
            var userId = cacheService.Get<Guid>(CacheHelper.AuthRecoverPasswordOTPKey(code));

            if (Guid.Empty == userId)
            {
                throw new NotFoundException("El código que ingresó, es incorrecto o expiró");
            }

            var user = await uow.userRepository.Get(x => x.Id == userId)
                ?? throw new NotFoundException("Imposible encontrar al usuario, para completar esta petición");

            user.Password = Hasher.HashPassword(model.NewPassword);
            await uow.userRepository.Update(user);
            var template = await emailTemplateService.Get(EmailTemplateNameConstants.AUTH_PASSWORD_CHANGED, []);
            await smtp.Send(user.Email, template.Subject, template.Body);

            await uow.SaveChangesAsync();

            return ResponseHelper.Create("Contraseña cambiada con éxito");
        }

        public async Task<GenericResponse<string>> ChangePassword(ChangePasswordAuthRequest model, Claim claim)
        {
            var executor = await userService.GetExecutor(claim.Value);

            if (!Hasher.ComparePassword(model.CurrentPassword, executor.Password))
            {
                throw new BadRequestException("La contraseña que argumentó como actual, es incorrecta.");
            }

            executor.Password = Hasher.HashPassword(model.NewPassword);

            await uow.userRepository.Update(executor);

            var template = await emailTemplateService.Get(EmailTemplateNameConstants.AUTH_PASSWORD_CHANGED, []);
            await smtp.Send(executor.Email, template.Subject, template.Body);

            await uow.SaveChangesAsync();

            return ResponseHelper.Create("Contraseña cambiada correctamente");
        }


    }
}
