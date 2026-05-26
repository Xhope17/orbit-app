using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<Message> Create(Message message);
    Task<Message?> Get(Guid id);
    Task<List<Message>> GetChatMessages(Guid chatId, int limit, int offset);
    Task<bool> IfExists(Guid id);
}
