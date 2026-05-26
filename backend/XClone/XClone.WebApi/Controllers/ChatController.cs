using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Chat;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatController(IChatService chatService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Enviar mensaje privado")]
    [EndpointDescription("Envía un mensaje privado a otro usuario. Si no existe un chat entre ambos, se crea automáticamente.")]
    [ProducesResponseType<GenericResponse<ChatDto>>(StatusCodes.Status200OK)]
    [Tags("chat", "messages")]
    public async Task<GenericResponse<ChatDto>> SendMessage([FromBody] SendMessageRequest model)
    {
        var rsp = await chatService.SendMessage(model, UserClaim());
        return rsp;
    }

    [HttpGet("mine")]
    [EndpointSummary("Obtener mis chats")]
    [EndpointDescription("Devuelve todos los chats del usuario autenticado, con el último mensaje de cada uno.")]
    [ProducesResponseType<GenericResponse<List<ChatDto>>>(StatusCodes.Status200OK)]
    [Tags("chat", "messages")]
    public async Task<GenericResponse<List<ChatDto>>> GetMyChats()
    {
        return await chatService.GetMyChats(UserClaim());
    }

    [HttpGet("{chatId:guid}/messages")]
    [EndpointSummary("Obtener mensajes de un chat")]
    [EndpointDescription("Obtiene el historial de mensajes de un chat específico con paginación.")]
    [ProducesResponseType<GenericResponse<List<MessageDto>>>(StatusCodes.Status200OK)]
    [Tags("chat", "messages")]
    public async Task<GenericResponse<List<MessageDto>>> GetMessages(Guid chatId, [FromQuery] int limit = 50, [FromQuery] int offset = 0)
    {
        return await chatService.GetMessages(chatId, limit, offset, UserClaim());
    }

    private Claim UserClaim()
    {
        return User.FindFirst(ClaimsConstants.USER_ID)
            ?? throw new BadRequestException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
    }
}
