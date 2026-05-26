using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface IReplyRepository
{
    Task<Reply> Create(Reply reply);
    Task<Reply?> Get(Guid id);
    Task<bool> Delete(Guid id);
    Task<List<Reply>> GetByPost(Guid postId);
    Task<bool> IfExists(Guid id);
    IQueryable<Reply> Queryable();
}
