using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Reply;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services;

public interface IReplyService
{
    Task<GenericResponse<ReplyDto>> Create(CreateReplyRequest model, Claim claim);
    Task<GenericResponse<List<ReplyDto>>> GetByPost(Guid postId);
    Task<GenericResponse<bool>> Delete(Guid id, Claim claim);
}
