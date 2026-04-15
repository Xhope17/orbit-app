using XClone.Application.Models.Requets.Auth;
using XClone.Application.Models.Responses;
using XClone.Application.Models.Responses.Auth;

namespace XClone.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<GenericResponse<LoginAuthResponse>> Login(LoginAuthRequest model);
        Task<GenericResponse<LoginAuthResponse>> Renew(RenewAuthRequest model);
    }
}
