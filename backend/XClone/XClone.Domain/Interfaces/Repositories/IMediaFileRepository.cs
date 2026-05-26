using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface IMediaFileRepository
{
    Task<MediaFile> Create(MediaFile mediaFile);
    Task<MediaFile?> GetByPublicId(string publicId);
    Task<bool> DeleteByPublicId(string publicId);
    IQueryable<MediaFile> Queryable();
}
