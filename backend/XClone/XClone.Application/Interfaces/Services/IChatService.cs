using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Chat;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services;

public interface IChatService
{
    Task<GenericResponse<ChatDto>> SendMessage(SendMessageRequest model, Claim claim);
    Task<GenericResponse<List<MessageDto>>> GetMessages(Guid chatId, int limit, int offset, Claim claim);
    Task<GenericResponse<List<ChatDto>>> GetMyChats(Claim claim);
}
