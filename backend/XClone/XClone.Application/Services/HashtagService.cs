using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Responses;
using XClone.Domain.DataBase.SqlServer;

namespace XClone.Application.Services;

public class HashtagService(IUnitOfWork uow) : IHashtagService
{
    public async Task<GenericResponse<List<HashtagDto>>> GetTrending(int count)
    {
        var hashtags = await uow.hashtagRepository.GetTrending(count);
        var dtos = hashtags.Select(h => new HashtagDto
        {
            Id = h.Id,
            Texto = h.Texto,
            PostCount = h.PostHashtags.Count
        }).ToList();
        return ResponseHelper.Create(dtos);
    }
}
