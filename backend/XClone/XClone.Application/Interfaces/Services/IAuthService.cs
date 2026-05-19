using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Auth;
using XClone.Application.Models.Requets.Auth.RecoverPassword;
using XClone.Application.Models.Requets.Auth.Register;
using XClone.Application.Models.Requets.User;
using XClone.Application.Models.Responses;
using XClone.Application.Models.Responses.Auth;

namespace XClone.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<GenericResponse<LoginAuthResponse>> Login(LoginAuthRequest model);
        Task<GenericResponse<LoginAuthResponse>> Renew(RenewAuthRequest model);

        // Register
        Task<GenericResponse<string>> RegisterInit(RegisterInitAuthRequest model);
        Task<GenericResponse<RegisterInitAuthResponse>> RegisterValidateToken(string token);
        Task<GenericResponse<UserDto>> RegisterComplete(CreateUserRequest model, string token);

        // Recover password
        Task<GenericResponse<string>> RecoverPasswordSendOTP(RecoverPasswordSendOTPAuthRequest model);
        Task<GenericResponse<string>> RecoverPassword(RecoverPasswordAuthRequest model, string code);
        Task<GenericResponse<string>> ChangePassword(ChangePasswordAuthRequest model, Claim claim);
    }
}
