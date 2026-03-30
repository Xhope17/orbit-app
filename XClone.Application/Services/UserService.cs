using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.User;
using XClone.Application.Models.Responses;
using XClone.Shared;
using XClone.Shared.Constants;
using XClone.Shared.Helpers;

namespace XClone.Application.Services
{
    public class UserService(Cache<UserDto> cache) : IUserService
    {

        //crear un usuario
        public GenericResponse<UserDto> Create(CreateUserRequest model)
        {

            //si es mayor de edad, se puede crear el usuario
            if (model.Edad < 18)
            {
                return ResponseHelper.Create<UserDto>(null, ValidationConstants.INVALID_AGE);
            }

            var user = new UserDto
            {
                UserId = Guid.NewGuid(),
                UserName = model.UserName,
                DisplayName = model.DisplayName,
                Edad = model.Edad,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTimeHelper.UtcNow(),
                JoinedAt = DateTimeHelper.UtcNow(),
                IsActive = true
            };

            cache.Add(user.UserId.ToString(), user);
            return ResponseHelper.Create(user, "Usuario creado");
        }

        //borrar
        public GenericResponse<bool> Delete(Guid userId)
        {
            var isDeleted = cache.Get(userId.ToString());

            if (isDeleted is null)
            {
                return ResponseHelper.Create(false);
            }
            cache.Delete(userId.ToString());

            return ResponseHelper.Create(true, "Usuario eliminado");
        }

        public GenericResponse<List<UserDto>> Get(int limit, int offset)
        {
            var users = cache.Get();

            return ResponseHelper.Create(users);
        }

        public GenericResponse<UserDto?> Get(Guid userId)
        {
            var user = cache.Get(userId.ToString());

            return ResponseHelper.Create(user, "Usuario encontrado");
        }

        public GenericResponse<UserDto> Update(Guid userId, UpdateUserRequest model)
        {
            var exist = cache.Get(userId.ToString());

            if (exist is null)
            {
                return ResponseHelper.Create<UserDto>(null!, ValidationConstants.USER_NOT_FOUND);
            }

            exist.UserName = model.UserName;
            exist.DisplayName = model.UserName;
            exist.Edad = model.Edad;
            exist.Email = model.Email;
            exist.PhoneNumber = model.PhoneNumber;

            cache.Update(userId.ToString(), exist);

            return ResponseHelper.Create(exist, "Usuario actualizado");
        }
    }
}
