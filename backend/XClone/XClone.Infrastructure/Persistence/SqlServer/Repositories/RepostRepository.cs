using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class RepostRepository(XcloneContext context) : IRepostRepository
{
    public async Task<Repost> Create(Repost repost)
    {
        await context.Reposts.AddAsync(repost);
        return repost;
    }

    public async Task<bool> Delete(Guid postId, Guid userId)
    {
        var entity = await context.Reposts
            .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);
        if (entity is null) return false;
        context.Reposts.Remove(entity);
        return true;
    }

    public async Task<Repost?> Get(Guid postId, Guid userId)
    {
        return await context.Reposts
            .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);
    }

    public async Task<bool> IfExists(Guid postId, Guid userId)
    {
        return await context.Reposts.AnyAsync(r => r.PostId == postId && r.UserId == userId);
    }

    public async Task<int> CountByPost(Guid postId)
    {
        return await context.Reposts.CountAsync(r => r.PostId == postId);
    }

    public IQueryable<Repost> Queryable()
    {
        return context.Reposts.AsQueryable();
    }
}
