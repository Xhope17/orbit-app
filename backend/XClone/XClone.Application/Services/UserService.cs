using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.User;
using XClone.Application.Models.Responses;
using XClone.Application.Queries;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Exceptions;
using XClone.Shared;
using XClone.Shared.Constants;
using XClone.Shared.Helpers;

namespace XClone.Application.Services
{
    public class UserService(IUnitOfWork uow, IConfiguration configuration, SMTP smtp, IEmailTemplateService emailTemplateService) : IUserService
    {

        //crear un usuario
        public async Task<GenericResponse<UserDto>> Create(CreateUserRequest model, Claim? claim)
        {
            Role? roleToAssign = null;
            User? executor = null;

            //
            // Normal use
            if (claim is not null)
            {
                executor = await GetExecutor(claim.Value);

                if (!model.RoleId.HasValue || model.RoleId.HasValue && model.RoleId.Value == Guid.Empty)
                {
                    throw new NotFoundException(ValidationConstants.IsEmpty("RoleId"));
                }

                await ValidateEmailIfExists(model.Email);

                roleToAssign = await ValidateRole(executor, model.RoleId.Value);
            }
            // Without authentication for register
            else
            {
                roleToAssign = await uow.roleRepository.Get(x => x.Name == RoleConstants.User);
            }

            if (roleToAssign is null)
            {
                throw new BadRequestException("Imposible obtener el rol para asignarle al usuario");
            }

            var defaultProfile = configuration[ConfigurationConstants.DEFAULT_PROFILE_PICTURE_URL]
                ?? "https://res.cloudinary.com/dm2sj1l7n/image/upload/v1/defaults/profile_default.png";
            var defaultBanner = configuration[ConfigurationConstants.DEFAULT_BANNER_PICTURE_URL]
                ?? "https://res.cloudinary.com/dm2sj1l7n/image/upload/v1/defaults/banner_default.png";

            var create = await uow.userRepository.Create(new User
            {
                UserName = model.UserName,
                DisplayName = model.DisplayName,
                Age = model.Edad,
                PhoneNumber = model.PhoneNumber,
                Position = model.Position,
                Email = model.Email,
                ProfilePictureUrl = model.ProfilePictureUrl ?? defaultProfile,
                BannerPictureUrl = model.BannerPictureUrl ?? defaultBanner,
                Password = Hasher.HashPassword(model.Password),
                UserRoleUsers = [
                    new UserRole {
                        RoleId = roleToAssign.Id,
                        AssignedBy = executor?.Id
                    }
                ]
            });

            await uow.SaveChangesAsync();
            return ResponseHelper.Create(Map(create));
        }



        //borrar
        public async Task<GenericResponse<bool>> Delete(Guid userId)
        {
            var user = await GetUser(userId);

            user.DeletedAt = DateTimeHelper.UtcNow();
            user.IsActive = false;

            await uow.userRepository.Update(user);
            await uow.SaveChangesAsync();

            return ResponseHelper.Create(true);
        }

        //Get all users
        public GenericResponse<List<UserDto>> Get(FilterUserRequest model)
        {
            var queryable = uow.userRepository.Queryable();

            var users = queryable
                .Include(user => user.UserRoleUsers)
                .ThenInclude(userRole => userRole.Role)
                .ApplyQuery(model)
                .AsQueryable()
                .Skip(model.Offset)
                .Take(model.Limit)
                .Select(user => Map(user))
                .ToList();

            return ResponseHelper.Create(users, count: queryable.Count());
        }

        //get user por id
        public async Task<GenericResponse<UserDto>> Get(Guid userId)
        {
            var user = await GetUser(userId);

            return ResponseHelper.Create(Map(user));
        }


        public async Task<GenericResponse<UserDto>> Update(Guid userId, UpdateUserRequest model, Claim claim)
        {
            var user = await GetUser(userId);
            var executor = await GetExecutor(claim.Value);

            // Solo actualizamos los campos que el request envía. 
            // Si viene null, conservamos el valor actual de la BD.
            user.UserName = model.UserName ?? user.UserName;
            user.DisplayName = model.DisplayName ?? user.DisplayName;
            user.Age = model.Age ?? user.Age;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

            user.UpdatedAt = DateTimeHelper.UtcNow();

            //user.Email = model.Email ?? user.Email;
            if (!string.IsNullOrWhiteSpace(model.Email) && user.Email != model.Email)
            {
                await ValidateEmailIfExists(model.Email);
                user.Email = model.Email;
            }

            if (model.RoleId.HasValue)
            {
                var roleToAssign = await ValidateRole(executor, model.RoleId.Value);

                await uow.userRepository.ClearRoles([.. user.UserRoleUsers]);

                user.UserRoleUsers.Add(new UserRole
                {
                    RoleId = roleToAssign.Id,
                    AssignedBy = executor.Id
                });
            }

            var update = await uow.userRepository.Update(user);
            await uow.SaveChangesAsync();
            return ResponseHelper.Create(Map(update));
        }

        //peticiones internas
        private async Task<User> GetUser(Guid userId)
        {
            return await uow.userRepository.Get(userId)
                ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST); // Asegúrate de tener esta constante
        }

        //map
        private static UserDto Map(User user)
        {
            var role = user.UserRoleUsers?.FirstOrDefault()?.Role;
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Age = user.Age,
                PhoneNumber = user.PhoneNumber,
                ProfilePictureUrl = user.ProfilePictureUrl,
                BannerPictureUrl = user.BannerPictureUrl,
                IsVerified = user.IsVerified,
                PinnedPostId = user.PinnedPostId,
                TimezoneId = user.TimezoneId,
                CityId = user.CityId,
                JoinedAt = user.JoinedAt,
                IsActive = user.IsActive,
                CreateAt = user.CreateAt,
                UpdatedAt = user.UpdatedAt ?? DateTime.MinValue,
                DeletedAt = user.DeletedAt,
                Role = role != null ? new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                } : null
            };
        }


        public async Task<GenericResponse<UserDto>> UpdateProfilePicture(UpdatePictureRequest model, Claim claim)
        {
            var executor = await GetExecutor(claim.Value);
            executor.ProfilePictureUrl = model.Url;
            executor.UpdatedAt = DateTimeHelper.UtcNow();

            await uow.userRepository.Update(executor);
            await uow.SaveChangesAsync();
            return ResponseHelper.Create(Map(executor));
        }

        public async Task<GenericResponse<UserDto>> UpdateBannerPicture(UpdatePictureRequest model, Claim claim)
        {
            var executor = await GetExecutor(claim.Value);
            executor.BannerPictureUrl = model.Url;
            executor.UpdatedAt = DateTimeHelper.UtcNow();

            await uow.userRepository.Update(executor);
            await uow.SaveChangesAsync();
            return ResponseHelper.Create(Map(executor));
        }

        public async Task CreateFirstUser()
        {
            var hasCreated = await uow.userRepository.HasCreated();
            if (hasCreated) return;

            var userName = configuration[ConfigurationConstants.FIRST_APP_TIME_USER_USERNAME]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.FIRST_APP_TIME_USER_USERNAME));

            var position = configuration[ConfigurationConstants.FIRST_APP_TIME_USER_POSITION]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.FIRST_APP_TIME_USER_POSITION));

            var displayName = configuration[ConfigurationConstants.FIRST_APP_TIME_USER_DISPLAYNAME]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.FIRST_APP_TIME_USER_POSITION));


            var email = configuration[ConfigurationConstants.FIRST_APP_TIME_USER_EMAIL]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.FIRST_APP_TIME_USER_EMAIL));

            var password = configuration[ConfigurationConstants.FIRST_APP_TIME_USER_PASSWORD]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.FIRST_APP_TIME_USER_PASSWORD));

            var adminRole = await uow.roleRepository.Get(x => x.Name == RoleConstants.Admin)
                ?? throw new Exception(ResponseConstants.RoleNotFound(RoleConstants.Admin));

            await uow.userRepository.Create(new User
            {
                UserName = userName,
                DisplayName = displayName,
                Email = email,
                Position = position,
                Password = Hasher.HashPassword(password),
                UserRoleUsers = [
                    new UserRole
                    {
                        RoleId = adminRole.Id,
                    }
                ]
            });
            await uow.SaveChangesAsync();
        }

        public async Task<User> GetExecutor(string value)
        {
            var uuid = Guid.Parse(value);
            return await uow.userRepository.Get(uuid)
                ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST);
        }

        private async Task ValidateEmailIfExists(string email)
        {
            if (await uow.userRepository.IfExists(x => x.Email == email))
            {
                throw new BadRequestException(ResponseConstants.USER_EMAIL_TAKED);
            }
        }

        private async Task<Role> ValidateRole(User executor, Guid roleId)
        {
            var roleToAssign = await uow.roleRepository.Get(roleId)
                ?? throw new NotFoundException(ResponseConstants.RoleNotFound(roleId));

            //si un moderador intenta asignar el rol de admin, se lanza una excepción porque no tiene permisos para asignar ese rol
            if (executor.UserRoleUsers.First().Role.Name == RoleConstants.Moderator && roleToAssign.Name == RoleConstants.Admin)

            {
                throw new BadRequestException(ResponseConstants.CANNOT_ASSIGN_THE_ROLE);
            }

            return roleToAssign;
        }

        public async Task<GenericResponse<UserDto>> Me(Claim claim)
        {
            var executor = await GetExecutor(claim.Value);
            return ResponseHelper.Create(Map(executor));
        }
    }
}
