using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FeedController(IFeedService feedService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Obtener feed personalizado")]
    [EndpointDescription("Devuelve los posts de los usuarios que sigue el usuario autenticado, ordenados por fecha descendente.")]
    [ProducesResponseType<GenericResponse<List<PostDto>>>(StatusCodes.Status200OK)]
    [Tags("feed", "timeline")]
    public async Task<GenericResponse<List<PostDto>>> GetFeed([FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        return await feedService.GetFeed(UserClaim(), limit, offset);
    }

    private Claim UserClaim()
    {
        return User.FindFirst(ClaimsConstants.USER_ID)
            ?? throw new BadRequestException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
    }
}
