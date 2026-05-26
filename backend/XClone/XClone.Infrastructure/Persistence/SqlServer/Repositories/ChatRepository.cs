using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class ChatRepository(XcloneContext context) : IChatRepository
{
    public async Task<Chat> Create(Chat chat)
    {
        await context.Chats.AddAsync(chat);
        return chat;
    }

    public async Task<Chat?> Get(Guid id)
    {
        return await context.Chats
            .Include(c => c.UserLow)
            .Include(c => c.UserHigh)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Chat?> GetByUsers(Guid userLowId, Guid userHighId)
    {
        return await context.Chats
            .Include(c => c.UserLow)
            .Include(c => c.UserHigh)
            .FirstOrDefaultAsync(c =>
                (c.UserLowId == userLowId && c.UserHighId == userHighId) ||
                (c.UserLowId == userHighId && c.UserHighId == userLowId));
    }

    public async Task<List<Chat>> GetUserChats(Guid userId)
    {
        return await context.Chats
            .Include(c => c.UserLow)
            .Include(c => c.UserHigh)
            .Where(c => c.UserLowId == userId || c.UserHighId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> IfExists(Guid id)
    {
        return await context.Chats.AnyAsync(c => c.Id == id);
    }

    public IQueryable<Chat> Queryable()
    {
        return context.Chats.AsQueryable();
    }
}
