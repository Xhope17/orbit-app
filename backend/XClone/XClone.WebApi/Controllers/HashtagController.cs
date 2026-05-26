using Microsoft.AspNetCore.Mvc;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Responses;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HashtagController(IHashtagService hashtagService) : ControllerBase
{
    [HttpGet("trending")]
    [EndpointSummary("Hashtags tendencia")]
    [EndpointDescription("Devuelve los hashtags más usados en la plataforma.")]
    [ProducesResponseType<GenericResponse<List<HashtagDto>>>(StatusCodes.Status200OK)]
    [Tags("hashtag", "trending")]
    public async Task<GenericResponse<List<HashtagDto>>> GetTrending([FromQuery] int count = 10)
    {
        return await hashtagService.GetTrending(count);
    }
}
