using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.Requets.Repost;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RepostController(IRepostService repostService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Repostear un post")]
    [EndpointDescription("Comparte un post en el perfil del usuario autenticado.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("repost", "interaction")]
    public async Task<GenericResponse<bool>> Repost([FromBody] CreateRepostRequest model)
    {
        return await repostService.Repost(model, UserClaim());
    }

    [HttpDelete("{postId:guid}")]
    [EndpointSummary("Eliminar repost")]
    [EndpointDescription("Elimina un repost del usuario autenticado.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("repost", "interaction")]
    public async Task<GenericResponse<bool>> Unrepost(Guid postId)
    {
        return await repostService.Unrepost(postId, UserClaim());
    }

    [HttpGet("{postId:guid}/count")]
    [AllowAnonymous]
    [EndpointSummary("Contar reposts de un post")]
    [EndpointDescription("Devuelve el número total de reposts de un post.")]
    [ProducesResponseType<GenericResponse<int>>(StatusCodes.Status200OK)]
    [Tags("repost", "interaction")]
    public async Task<GenericResponse<int>> CountByPost(Guid postId)
    {
        return await repostService.CountByPost(postId);
    }

    private Claim UserClaim()
    {
        return User.FindFirst(ClaimsConstants.USER_ID)
            ?? throw new BadRequestException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
    }
}
