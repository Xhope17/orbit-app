using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class MessageRepository(XcloneContext context) : IMessageRepository
{
    public async Task<Message> Create(Message message)
    {
        await context.Messages.AddAsync(message);
        return message;
    }

    public async Task<Message?> Get(Guid id)
    {
        return await context.Messages
            .Include(m => m.Sender)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<Message>> GetChatMessages(Guid chatId, int limit, int offset)
    {
        return await context.Messages
            .Include(m => m.Sender)
            .Where(m => m.ChatId == chatId)
            .OrderByDescending(m => m.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<bool> IfExists(Guid id)
    {
        return await context.Messages.AnyAsync(m => m.Id == id);
    }
}
