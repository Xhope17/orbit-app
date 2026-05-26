using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Quote;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuoteController(IQuoteService quoteService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Citar un post")]
    [EndpointDescription("Crea una cita de un post con texto adicional.")]
    [ProducesResponseType<GenericResponse<QuoteDto>>(StatusCodes.Status200OK)]
    [Tags("quote", "interaction")]
    public async Task<GenericResponse<QuoteDto>> Create([FromBody] CreateQuoteRequest model)
    {
        return await quoteService.Create(model, UserClaim());
    }

    [HttpGet("{postId:guid}")]
    [AllowAnonymous]
    [EndpointSummary("Obtener citas de un post")]
    [EndpointDescription("Devuelve todas las citas de un post.")]
    [ProducesResponseType<GenericResponse<List<QuoteDto>>>(StatusCodes.Status200OK)]
    [Tags("quote", "interaction")]
    public async Task<GenericResponse<List<QuoteDto>>> GetByPost(Guid postId)
    {
        return await quoteService.GetByPost(postId);
    }

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Eliminar cita")]
    [EndpointDescription("Elimina una cita. Solo el autor puede eliminarla.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("quote", "interaction")]
    public async Task<GenericResponse<bool>> Delete(Guid id)
    {
        return await quoteService.Delete(id, UserClaim());
    }

    private Claim UserClaim()
    {
        return User.FindFirst(ClaimsConstants.USER_ID)
            ?? throw new BadRequestException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
    }
}
