using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class FollowingRepository(XcloneContext context) : IFollowingRepository
{
    public async Task<Following> Create(Following following)
    {
        await context.Followings.AddAsync(following);
        return following;
    }

    public async Task<bool> Delete(Guid followerId, Guid followedId)
    {
        var entity = await context.Followings
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowedId == followedId);
        if (entity is null) return false;
        context.Followings.Remove(entity);
        return true;
    }

    public async Task<Following?> Get(Guid followerId, Guid followedId)
    {
        return await context.Followings
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowedId == followedId);
    }

    public async Task<List<Following>> GetFollowers(Guid userId)
    {
        return await context.Followings
            .Include(f => f.Follower)
            .Where(f => f.FollowedId == userId)
            .ToListAsync();
    }

    public async Task<List<Following>> GetFollowed(Guid userId)
    {
        return await context.Followings
            .Include(f => f.Followed)
            .Where(f => f.FollowerId == userId)
            .ToListAsync();
    }

    public async Task<bool> IfExists(Guid followerId, Guid followedId)
    {
        return await context.Followings
            .AnyAsync(f => f.FollowerId == followerId && f.FollowedId == followedId);
    }

    public async Task<int> CountFollowers(Guid userId)
    {
        return await context.Followings.CountAsync(f => f.FollowedId == userId);
    }

    public async Task<int> CountFollowed(Guid userId)
    {
        return await context.Followings.CountAsync(f => f.FollowerId == userId);
    }

    public IQueryable<Following> Queryable()
    {
        return context.Followings.AsQueryable();
    }
}
