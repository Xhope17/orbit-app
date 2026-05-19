using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services
{
    public interface IAppService
    {
        Task<GenericResponse<AppInfoDto>> Info();
        Task<GenericResponse<List<MenuDto>>> Menu(Claim claim);
    }
}
