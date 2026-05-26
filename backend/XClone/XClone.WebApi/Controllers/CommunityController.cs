using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Community;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommunityController(ICommunityService communityService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Crear comunidad")]
    [EndpointDescription("Crea una nueva comunidad. El creador obtiene el rol Owner automáticamente.")]
    [ProducesResponseType<GenericResponse<CommunityDto>>(StatusCodes.Status200OK)]
    [Tags("community")]
    public async Task<GenericResponse<CommunityDto>> Create([FromBody] CreateCommunityRequest model)
    {
        return await communityService.Create(model, UserClaim());
    }

    [HttpGet]
    [AllowAnonymous]
    [EndpointSummary("Listar comunidades")]
    [EndpointDescription("Obtiene todas las comunidades disponibles.")]
    [ProducesResponseType<GenericResponse<List<CommunityDto>>>(StatusCodes.Status200OK)]
    [Tags("community")]
    public async Task<GenericResponse<List<CommunityDto>>> GetAll()
    {
        return await communityService.GetAll();
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [EndpointSummary("Obtener comunidad")]
    [EndpointDescription("Obtiene una comunidad por su ID.")]
    [ProducesResponseType<GenericResponse<CommunityDto>>(StatusCodes.Status200OK)]
    [Tags("community")]
    public async Task<GenericResponse<CommunityDto>> Get(Guid id)
    {
        return await communityService.Get(id);
    }

    [HttpPut("{id:guid}")]
    [EndpointSummary("Actualizar comunidad")]
    [EndpointDescription("Actualiza el nombre y/o descripción de una comunidad. Solo el creador puede hacerlo.")]
    [ProducesResponseType<GenericResponse<CommunityDto>>(StatusCodes.Status200OK)]
    [Tags("community")]
    public async Task<GenericResponse<CommunityDto>> Update(Guid id, [FromBody] UpdateCommunityRequest model)
    {
        return await communityService.Update(id, model, UserClaim());
    }

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Eliminar comunidad")]
    [EndpointDescription("Elimina una comunidad. Solo el creador puede hacerlo.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("community")]
    public async Task<GenericResponse<bool>> Delete(Guid id)
    {
        return await communityService.Delete(id, UserClaim());
    }

    [HttpPost("{id:guid}/join")]
    [EndpointSummary("Unirse a comunidad")]
    [EndpointDescription("Permite al usuario autenticado unirse a una comunidad como miembro.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("community", "members")]
    public async Task<GenericResponse<bool>> Join(Guid id)
    {
        return await communityService.Join(id, UserClaim());
    }

    [HttpPost("{id:guid}/leave")]
    [EndpointSummary("Salir de comunidad")]
    [EndpointDescription("Permite al usuario autenticado salir de una comunidad.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("community", "members")]
    public async Task<GenericResponse<bool>> Leave(Guid id)
    {
        return await communityService.Leave(id, UserClaim());
    }

    [HttpGet("{id:guid}/members")]
    [AllowAnonymous]
    [EndpointSummary("Miembros de comunidad")]
    [EndpointDescription("Obtiene la lista de miembros de una comunidad.")]
    [ProducesResponseType<GenericResponse<List<CommunityMemberDto>>>(StatusCodes.Status200OK)]
    [Tags("community", "members")]
    public async Task<GenericResponse<List<CommunityMemberDto>>> GetMembers(Guid id)
    {
        return await communityService.GetMembers(id);
    }

    private Claim UserClaim()
    {
        return User.FindFirst(ClaimsConstants.USER_ID)
            ?? throw new BadRequestException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
    }
}
