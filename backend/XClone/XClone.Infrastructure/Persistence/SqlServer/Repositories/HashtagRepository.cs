using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class HashtagRepository(XcloneContext context) : IHashtagRepository
{
    public async Task<Hashtag> Create(Hashtag hashtag)
    {
        await context.Hashtags.AddAsync(hashtag);
        return hashtag;
    }

    public async Task<Hashtag?> GetByText(string text)
    {
        return await context.Hashtags
            .FirstOrDefaultAsync(h => h.Texto == text);
    }

    public async Task<List<Hashtag>> GetTrending(int count)
    {
        return await context.Hashtags
            .OrderByDescending(h => h.PostHashtags.Count)
            .Take(count)
            .ToListAsync();
    }

    public async Task<bool> IfExists(string text)
    {
        return await context.Hashtags.AnyAsync(h => h.Texto == text);
    }

    public IQueryable<Hashtag> Queryable()
    {
        return context.Hashtags.AsQueryable();
    }
}
