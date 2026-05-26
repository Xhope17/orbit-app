using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Following;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services;

public interface IFollowingService
{
    Task<GenericResponse<FollowingDto>> Follow(FollowRequest model, Claim claim);
    Task<GenericResponse<bool>> Unfollow(Guid userId, Claim claim);
    Task<GenericResponse<List<UserDto>>> GetFollowers(Claim claim);
    Task<GenericResponse<List<UserDto>>> GetFollowing(Claim claim);
    Task<GenericResponse<object>> GetCounts(Claim claim);
}
