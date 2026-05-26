using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface IFollowingRepository
{
    Task<Following> Create(Following following);
    Task<bool> Delete(Guid followerId, Guid followedId);
    Task<Following?> Get(Guid followerId, Guid followedId);
    Task<List<Following>> GetFollowers(Guid userId);
    Task<List<Following>> GetFollowed(Guid userId);
    Task<bool> IfExists(Guid followerId, Guid followedId);
    Task<int> CountFollowers(Guid userId);
    Task<int> CountFollowed(Guid userId);
    IQueryable<Following> Queryable();
}
