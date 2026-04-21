using Microsoft.AspNetCore.Mvc;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.Requets.Auth;
using XClone.Application.Models.Responses;
using XClone.Application.Models.Responses.Auth;
using XClone.WebApi.Helpers;

namespace XClone.WebApi.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("login")]
        [EndpointSummary("Iniciar sesión como usuario")]
        [EndpointDescription("Esto permite iniciar sesión en el aplicativo. Genera dos tokens, uno que es el JWT para la autenticación y el otro, que permite realizar la renovación")]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<GenericResponse<LoginAuthResponse>>(StatusCodes.Status200OK)]
        //[]
        [Tags("auth", "user", "JWT", "Refresh_Token")]
        public async Task<GenericResponse<LoginAuthResponse>> Login([FromBody] LoginAuthRequest model)
        {
            var rsp = await service.Login(model);
            return ResponseStatus.Created(HttpContext, rsp);
        }

        [HttpPost("renew")]
        [EndpointSummary("Renovar sesión")]
        [EndpointDescription("Esto le permite renovar la sesión en el aplicativo. Genera dos tokens, uno que es el JWT para la autenticación con el aplicativo, y otro, que es, el que le permite realizar la renovación.")]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<GenericResponse<LoginAuthResponse>>(StatusCodes.Status200OK)]
        [Tags("auth", "users", "jwt", "refresh_token")]
        public async Task<GenericResponse<LoginAuthResponse>> Renew([FromBody] RenewAuthRequest model)
        {
            var srv = await service.Renew(model);
            return ResponseStatus.Created.Ok(HttpContext, srv);
        }
    }
}
