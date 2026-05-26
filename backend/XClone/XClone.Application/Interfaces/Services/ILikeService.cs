using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Like;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services;

public interface ILikeService
{
    Task<GenericResponse<bool>> Like(LikeRequest model, Claim claim);
    Task<GenericResponse<bool>> Unlike(Guid postId, Claim claim);
    Task<GenericResponse<int>> CountByPost(Guid postId);
}
