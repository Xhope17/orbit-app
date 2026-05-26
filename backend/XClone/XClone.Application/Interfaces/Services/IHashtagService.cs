using XClone.Application.Models.DTOs;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services;

public interface IHashtagService
{
    Task<GenericResponse<List<HashtagDto>>> GetTrending(int count);
}
