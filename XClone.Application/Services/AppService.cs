using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Responses;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.DataBase.SqlServer;
using XClone.Shared.Constants;

namespace XClone.Application.Services
{
    public class AppService(IConfiguration configuration, IUnitOfWork uow, IUserService userService) : IAppService
    {
        public async Task<GenericResponse<AppInfoDto>> Info()
        {
            return ResponseHelper.Create(new AppInfoDto
            {
                Version = configuration[ConfigurationConstants.VERSION] ?? "0.0.0",
                Roles = [.. uow.roleRepository.Queryable().Where(x => x.IsActive).ToList().Select(r => MapRole(r))]
            });
        }

        public async Task<GenericResponse<List<MenuDto>>> Menu(Claim claim)
        {
            var executor = await userService.GetExecutor(claim.Value);
            var menu = await uow.userRepository.GetMenu(executor.Id);

            return ResponseHelper.Create(menu.Select(m => MapMenu(m)).ToList());
        }

        private RoleDto MapRole(Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
            };
        }

        private MenuDto MapMenu(Menu menu)
        {
            return new MenuDto
            {
                Id = menu.Id,
                Code = menu.Code,
                Path = menu.Path,
                IconName = menu.IconName,
                Name = menu.Name,
                ParentId = menu.ParentId,
            };
        }
    }
}
