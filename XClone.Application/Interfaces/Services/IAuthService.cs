using XClone.Application.Models.Requets.Auth;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<GenericResponse<string>> Login(LoginAuthRequest model);
    }
}
