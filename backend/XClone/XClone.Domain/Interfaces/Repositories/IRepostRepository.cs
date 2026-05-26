using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface IRepostRepository
{
    Task<Repost> Create(Repost repost);
    Task<bool> Delete(Guid postId, Guid userId);
    Task<Repost?> Get(Guid postId, Guid userId);
    Task<bool> IfExists(Guid postId, Guid userId);
    Task<int> CountByPost(Guid postId);
    IQueryable<Repost> Queryable();
}
