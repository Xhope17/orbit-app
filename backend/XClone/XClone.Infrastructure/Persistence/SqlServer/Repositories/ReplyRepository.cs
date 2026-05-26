using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class ReplyRepository(XcloneContext context) : IReplyRepository
{
    public async Task<Reply> Create(Reply reply)
    {
        await context.Replies.AddAsync(reply);
        return reply;
    }

    public async Task<Reply?> Get(Guid id)
    {
        return await context.Replies
            .Include(r => r.Author)
            .Include(r => r.Post)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await context.Replies.FindAsync(id);
        if (entity is null) return false;
        context.Replies.Remove(entity);
        return true;
    }

    public async Task<List<Reply>> GetByPost(Guid postId)
    {
        return await context.Replies
            .Include(r => r.Author)
            .Where(r => r.PostId == postId)
            .OrderBy(r => r.FechaCreacion)
            .ToListAsync();
    }

    public async Task<bool> IfExists(Guid id)
    {
        return await context.Replies.AnyAsync(r => r.Id == id);
    }

    public IQueryable<Reply> Queryable()
    {
        return context.Replies.AsQueryable();
    }
}
