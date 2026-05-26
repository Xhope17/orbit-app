using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Block;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BlockController(IBlockService blockService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Bloquear a un usuario")]
    [EndpointDescription("Bloquea a un usuario para evitar interacción.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("block")]
    public async Task<GenericResponse<bool>> BlockUser([FromBody] BlockRequest model)
    {
        return await blockService.BlockUser(model, UserClaim());
    }

    [HttpDelete("{userId:guid}")]
    [EndpointSummary("Desbloquear a un usuario")]
    [EndpointDescription("Elimina el bloqueo de un usuario.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("block")]
    public async Task<GenericResponse<bool>> UnblockUser(Guid userId)
    {
        return await blockService.UnblockUser(userId, UserClaim());
    }

    [HttpGet]
    [EndpointSummary("Lista de usuarios bloqueados")]
    [EndpointDescription("Devuelve la lista de usuarios bloqueados por el usuario autenticado.")]
    [ProducesResponseType<GenericResponse<List<UserDto>>>(StatusCodes.Status200OK)]
    [Tags("block")]
    public async Task<GenericResponse<List<UserDto>>> GetBlockedUsers()
    {
        return await blockService.GetBlockedUsers(UserClaim());
    }

    private Claim UserClaim()
    {
        return User.FindFirst(ClaimsConstants.USER_ID)
            ?? throw new BadRequestException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
    }
}
