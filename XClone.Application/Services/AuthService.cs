using Microsoft.Extensions.Configuration;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.Requets.Auth;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Application.Services
{
    public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
    {
        public async Task<GenericResponse<string>> Login(LoginAuthRequest model)
        {
            var user = await userRepository.Get(model.email)
                ?? throw new BadRequestException("Usuario o contraeeña incorrectos");
        }
    }
}
