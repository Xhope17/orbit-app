using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.Requets.Like;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LikeController(ILikeService likeService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Dar like a un post")]
    [EndpointDescription("Permite al usuario autenticado dar like a un post.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("like", "interaction")]
    public async Task<GenericResponse<bool>> Like([FromBody] LikeRequest model)
    {
        return await likeService.Like(model, UserClaim());
    }

    [HttpDelete("{postId:guid}")]
    [EndpointSummary("Quitar like")]
    [EndpointDescription("Permite al usuario autenticado quitar el like de un post.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("like", "interaction")]
    public async Task<GenericResponse<bool>> Unlike(Guid postId)
    {
        return await likeService.Unlike(postId, UserClaim());
    }

    [HttpGet("{postId:guid}/count")]
    [AllowAnonymous]
    [EndpointSummary("Contar likes de un post")]
    [EndpointDescription("Devuelve el número total de likes de un post.")]
    [ProducesResponseType<GenericResponse<int>>(StatusCodes.Status200OK)]
    [Tags("like", "interaction")]
    public async Task<GenericResponse<int>> CountByPost(Guid postId)
    {
        return await likeService.CountByPost(postId);
    }

    private Claim UserClaim()
    {
        return User.FindFirst(ClaimsConstants.USER_ID)
            ?? throw new BadRequestException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
    }
}
