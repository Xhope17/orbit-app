using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.User;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;
using XClone.WebApi.Attributes;
using XClone.WebApi.Helpers;

namespace XClone.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [DeveloperAuthor(Name = "Bryan M.", Description = "Esto lo cree para la información de la APP")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        //Crear
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [EndpointSummary("Crear un usuario")]
        [EndpointDescription("Este endpoint permite crear un nuevo usuario en el sistema. Requiere que el solicitante tenga el rol de 'Admin'.")]
        [ProducesResponseType<GenericResponse<UserDto>>(StatusCodes.Status201Created)]
        public async Task<GenericResponse<UserDto>> Create([FromBody] CreateUserRequest model)
        {
            var rsp = await userService.Create(model, UserClaim());
            return ResponseStatus.Created(HttpContext, rsp);

        }

        //obtener todos los post
        [HttpGet]
        [Authorize]
        [EndpointSummary("Obtener todos los usuarios")]
        [EndpointDescription("Este endpoint permite obtener todos los usuarios del sistema. Requiere que el solicitante esté autenticado.")]
        [ProducesResponseType<GenericResponse<List<UserDto>>>(StatusCodes.Status200OK)]
        public async Task<GenericResponse<List<UserDto>>> GetAll([FromQuery] FilterUserRequest model, [FromHeader] string authorization)
        {
            //var userId = User.FindFirst("id")?.Value;
            var rsp = userService.Get(model);


            return ResponseStatus.Ok(HttpContext, rsp);
        }

        //obtener un post
        [HttpGet("{id:guid}")]
        [Authorize]
        [EndpointSummary("Obtener un usuario por ID")]
        [EndpointDescription("Obtiene un usuario específico por su ID. Requiere que el solicitante esté autenticado.")]
        [ProducesResponseType<GenericResponse<UserDto>>(StatusCodes.Status200OK)]
        public async Task<GenericResponse<UserDto>> GetById(Guid id)
        {
            var rsp = await userService.Get(id);

            return ResponseStatus.Ok(HttpContext, rsp);
        }

        [HttpGet("me")]
        [Authorize]
        [EndpointSummary("Obtener información del usuario autenticado")]
        [EndpointDescription("Este endpoint devuelve la información del usuario actualmente autenticado. Requiere que el solicitante esté autenticado.")]
        [ProducesResponseType<GenericResponse<UserDto>>(StatusCodes.Status200OK)]
        public async Task<GenericResponse<UserDto>> Me()
        {
            var srv = await userService.Me(UserClaim());
            return ResponseStatus.Ok(HttpContext, srv);
        }

        //Actualizar
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [EndpointSummary("Actualizar un usuario")]
        [EndpointDescription("Este endpoint permite actualizar la información de un usuario existente. Requiere que el solicitante tenga el rol de 'Admin'.")]
        [ProducesResponseType<GenericResponse<UserDto>>(StatusCodes.Status200OK)]
        public async Task<GenericResponse<UserDto>> Update([FromBody] UpdateUserRequest model, Guid id)
        {
            var rsp = await userService.Update(id, model, UserClaim());

            return ResponseStatus.Updated(HttpContext, rsp);
        }

        //eliminar un usuario
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [EndpointSummary("Eliminar un usuario")]
        [EndpointDescription("Este endpoint permite eliminar un usuario del sistema. Requiere que el solicitante tenga el rol de 'Admin'.")]
        [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
        public async Task<GenericResponse<bool>> Delete(Guid id)
        {
            var rsp = await userService.Delete(id);
            return ResponseStatus.Ok(HttpContext, rsp);
        }


        private Claim UserClaim()
        {
            return User.FindFirst(ClaimsConstants.USER_ID)
                ?? throw new BadRequestException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
        }


    }
}
