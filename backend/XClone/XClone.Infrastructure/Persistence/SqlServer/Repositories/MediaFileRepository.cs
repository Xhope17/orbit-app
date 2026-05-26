using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class MediaFileRepository(XcloneContext context) : IMediaFileRepository
{
    public async Task<MediaFile> Create(MediaFile mediaFile)
    {
        await context.MediaFiles.AddAsync(mediaFile);
        return mediaFile;
    }

    public async Task<MediaFile?> GetByPublicId(string publicId)
    {
        return await context.MediaFiles
            .FirstOrDefaultAsync(m => m.PublicId == publicId);
    }

    public async Task<bool> DeleteByPublicId(string publicId)
    {
        var entity = await context.MediaFiles
            .FirstOrDefaultAsync(m => m.PublicId == publicId);
        if (entity is null) return false;
        context.MediaFiles.Remove(entity);
        return true;
    }

    public IQueryable<MediaFile> Queryable()
    {
        return context.MediaFiles.AsQueryable();
    }
}
