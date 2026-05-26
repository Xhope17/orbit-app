using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Reply;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReplyController(IReplyService replyService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Responder a un post")]
    [EndpointDescription("Crea una respuesta a un post existente.")]
    [ProducesResponseType<GenericResponse<ReplyDto>>(StatusCodes.Status200OK)]
    [Tags("reply", "interaction")]
    public async Task<GenericResponse<ReplyDto>> Create([FromBody] CreateReplyRequest model)
    {
        return await replyService.Create(model, UserClaim());
    }

    [HttpGet("{postId:guid}")]
    [AllowAnonymous]
    [EndpointSummary("Obtener respuestas de un post")]
    [EndpointDescription("Devuelve todas las respuestas de un post.")]
    [ProducesResponseType<GenericResponse<List<ReplyDto>>>(StatusCodes.Status200OK)]
    [Tags("reply", "interaction")]
    public async Task<GenericResponse<List<ReplyDto>>> GetByPost(Guid postId)
    {
        return await replyService.GetByPost(postId);
    }

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Eliminar respuesta")]
    [EndpointDescription("Elimina una respuesta. Solo el autor puede eliminarla.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("reply", "interaction")]
    public async Task<GenericResponse<bool>> Delete(Guid id)
    {
        return await replyService.Delete(id, UserClaim());
    }

    private Claim UserClaim()
    {
        return User.FindFirst(ClaimsConstants.USER_ID)
            ?? throw new BadRequestException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
    }
}
