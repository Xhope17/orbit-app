using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface IChatRepository
{
    Task<Chat> Create(Chat chat);
    Task<Chat?> Get(Guid id);
    Task<Chat?> GetByUsers(Guid userLowId, Guid userHighId);
    Task<List<Chat>> GetUserChats(Guid userId);
    Task<bool> IfExists(Guid id);
    IQueryable<Chat> Queryable();
}
