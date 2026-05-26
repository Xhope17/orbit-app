using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services;

public interface IFeedService
{
    Task<GenericResponse<List<PostDto>>> GetFeed(Claim claim, int limit, int offset);
}
