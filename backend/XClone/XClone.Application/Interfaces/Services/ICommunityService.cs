using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Community;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services;

public interface ICommunityService
{
    Task<GenericResponse<CommunityDto>> Create(CreateCommunityRequest model, Claim claim);
    Task<GenericResponse<CommunityDto>> Get(Guid id);
    Task<GenericResponse<List<CommunityDto>>> GetAll();
    Task<GenericResponse<CommunityDto>> Update(Guid id, UpdateCommunityRequest model, Claim claim);
    Task<GenericResponse<bool>> Delete(Guid id, Claim claim);
    Task<GenericResponse<bool>> Join(Guid communityId, Claim claim);
    Task<GenericResponse<bool>> Leave(Guid communityId, Claim claim);
    Task<GenericResponse<List<CommunityMemberDto>>> GetMembers(Guid communityId);
}
