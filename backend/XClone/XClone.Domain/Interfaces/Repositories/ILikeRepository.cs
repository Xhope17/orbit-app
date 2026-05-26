using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface ILikeRepository
{
    Task<Like> Create(Like like);
    Task<bool> Delete(Guid postId, Guid userId);
    Task<Like?> Get(Guid postId, Guid userId);
    Task<bool> IfExists(Guid postId, Guid userId);
    Task<int> CountByPost(Guid postId);
    IQueryable<Like> Queryable();
}
