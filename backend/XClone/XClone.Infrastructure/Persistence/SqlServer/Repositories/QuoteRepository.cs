using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class QuoteRepository(XcloneContext context) : IQuoteRepository
{
    public async Task<Quote> Create(Quote quote)
    {
        await context.Quotes.AddAsync(quote);
        return quote;
    }

    public async Task<Quote?> Get(Guid id)
    {
        return await context.Quotes
            .Include(q => q.Author)
            .Include(q => q.QuotedPost)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await context.Quotes.FindAsync(id);
        if (entity is null) return false;
        context.Quotes.Remove(entity);
        return true;
    }

    public async Task<List<Quote>> GetByPost(Guid postId)
    {
        return await context.Quotes
            .Include(q => q.Author)
            .Where(q => q.QuotedPostId == postId)
            .OrderByDescending(q => q.FechaCreacion)
            .ToListAsync();
    }

    public async Task<bool> IfExists(Guid id)
    {
        return await context.Quotes.AnyAsync(q => q.Id == id);
    }

    public IQueryable<Quote> Queryable()
    {
        return context.Quotes.AsQueryable();
    }
}
