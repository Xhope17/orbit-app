using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Repost;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services;

public interface IRepostService
{
    Task<GenericResponse<bool>> Repost(CreateRepostRequest model, Claim claim);
    Task<GenericResponse<bool>> Unrepost(Guid postId, Claim claim);
    Task<GenericResponse<int>> CountByPost(Guid postId);
}
