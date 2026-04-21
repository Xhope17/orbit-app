using Microsoft.AspNetCore.Mvc;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Responses;
using XClone.WebApi.Attributes;
using XClone.WebApi.Helpers;

namespace XClone.WebApi.Controllers
{
    [Route("api/[controller]")]
    [DeveloperAuthor(Name = "Bryan M.", Description = "Esto lo cree para la información de la APP")]
    public class AppController(IAppService appService) : ControllerBase
    {
        [HttpGet("info")]
        [EndpointSummary("Información de la aplicación")]
        [EndpointDescription("Los roles, permisos, versión y mas detalles de la aplicación")]
        [ProducesResponseType<GenericResponse<AppInfoDto>>(StatusCodes.Status200OK)]
        public async Task<GenericResponse<AppInfoDto>> Info()
        {
            var srv = await appService.Info();
            return ResponseStatus.Ok(HttpContext, srv);
        }
    }
}
