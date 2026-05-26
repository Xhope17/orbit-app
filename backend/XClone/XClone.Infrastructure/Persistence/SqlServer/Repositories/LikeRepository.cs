using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class LikeRepository(XcloneContext context) : ILikeRepository
{
    public async Task<Like> Create(Like like)
    {
        await context.Likes.AddAsync(like);
        return like;
    }

    public async Task<bool> Delete(Guid postId, Guid userId)
    {
        var entity = await context.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
        if (entity is null) return false;
        context.Likes.Remove(entity);
        return true;
    }

    public async Task<Like?> Get(Guid postId, Guid userId)
    {
        return await context.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
    }

    public async Task<bool> IfExists(Guid postId, Guid userId)
    {
        return await context.Likes.AnyAsync(l => l.PostId == postId && l.UserId == userId);
    }

    public async Task<int> CountByPost(Guid postId)
    {
        return await context.Likes.CountAsync(l => l.PostId == postId);
    }

    public IQueryable<Like> Queryable()
    {
        return context.Likes.AsQueryable();
    }
}
