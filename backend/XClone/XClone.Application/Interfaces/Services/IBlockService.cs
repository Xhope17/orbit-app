using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Block;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services;

public interface IBlockService
{
    Task<GenericResponse<bool>> BlockUser(BlockRequest model, Claim claim);
    Task<GenericResponse<bool>> UnblockUser(Guid userId, Claim claim);
    Task<GenericResponse<List<UserDto>>> GetBlockedUsers(Claim claim);
}
