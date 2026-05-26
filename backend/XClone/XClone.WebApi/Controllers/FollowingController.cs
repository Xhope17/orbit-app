using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Following;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FollowingController(IFollowingService followingService) : ControllerBase
{
    [HttpPost("follow")]
    [EndpointSummary("Seguir a un usuario")]
    [EndpointDescription("Permite al usuario autenticado seguir a otro usuario.")]
    [ProducesResponseType<GenericResponse<FollowingDto>>(StatusCodes.Status200OK)]
    [Tags("following", "followers")]
    public async Task<GenericResponse<FollowingDto>> Follow([FromBody] FollowRequest model)
    {
        return await followingService.Follow(model, UserClaim());
    }

    [HttpDelete("unfollow/{userId:guid}")]
    [EndpointSummary("Dejar de seguir a un usuario")]
    [EndpointDescription("Permite al usuario autenticado dejar de seguir a otro usuario.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("following", "followers")]
    public async Task<GenericResponse<bool>> Unfollow(Guid userId)
    {
        return await followingService.Unfollow(userId, UserClaim());
    }

    [HttpGet("followers")]
    [EndpointSummary("Obtener seguidores")]
    [EndpointDescription("Devuelve la lista de usuarios que siguen al usuario autenticado.")]
    [ProducesResponseType<GenericResponse<List<UserDto>>>(StatusCodes.Status200OK)]
    [Tags("following", "followers")]
    public async Task<GenericResponse<List<UserDto>>> GetFollowers()
    {
        return await followingService.GetFollowers(UserClaim());
    }

    [HttpGet("following")]
    [EndpointSummary("Obtener seguidos")]
    [EndpointDescription("Devuelve la lista de usuarios a los que sigue el usuario autenticado.")]
    [ProducesResponseType<GenericResponse<List<UserDto>>>(StatusCodes.Status200OK)]
    [Tags("following", "followers")]
    public async Task<GenericResponse<List<UserDto>>> GetFollowing()
    {
        return await followingService.GetFollowing(UserClaim());
    }

    [HttpGet("counts")]
    [EndpointSummary("Contar seguidores y seguidos")]
    [EndpointDescription("Devuelve el número de seguidores y de usuarios seguidos por el usuario autenticado.")]
    [ProducesResponseType<GenericResponse<object>>(StatusCodes.Status200OK)]
    [Tags("following", "followers")]
    public async Task<GenericResponse<object>> GetCounts()
    {
        return await followingService.GetCounts(UserClaim());
    }

    private Claim UserClaim()
    {
        return User.FindFirst(ClaimsConstants.USER_ID)
            ?? throw new BadRequestException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
    }
}
