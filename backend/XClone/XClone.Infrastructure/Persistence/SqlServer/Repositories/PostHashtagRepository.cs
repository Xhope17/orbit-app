using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class PostHashtagRepository(XcloneContext context) : IPostHashtagRepository
{
    public async Task<PostHashtag> Create(PostHashtag postHashtag)
    {
        await context.PostHashtags.AddAsync(postHashtag);
        return postHashtag;
    }

    public async Task<List<PostHashtag>> GetByPost(Guid postId)
    {
        return await context.PostHashtags
            .Include(ph => ph.Hashtag)
            .Where(ph => ph.PostId == postId)
            .ToListAsync();
    }

    public async Task<List<PostHashtag>> GetByHashtag(Guid hashtagId)
    {
        return await context.PostHashtags
            .Include(ph => ph.Post)
            .Where(ph => ph.HashtagId == hashtagId)
            .ToListAsync();
    }

    public async Task<bool> IfExists(Guid postId, Guid hashtagId)
    {
        return await context.PostHashtags
            .AnyAsync(ph => ph.PostId == postId && ph.HashtagId == hashtagId);
    }

    public IQueryable<PostHashtag> Queryable()
    {
        return context.PostHashtags.AsQueryable();
    }
}
