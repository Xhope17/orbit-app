using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class CommunityRepository(XcloneContext context) : ICommunityRepository
{
    public async Task<Community> Create(Community community)
    {
        await context.Communities.AddAsync(community);
        return community;
    }

    public async Task<Community?> Get(Guid id)
    {
        return await context.Communities
            .Include(c => c.Creator)
            .Include(c => c.CommunityMembers)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Community> Update(Community community)
    {
        context.Communities.Update(community);
        return community;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await context.Communities.FindAsync(id);
        if (entity is null) return false;
        context.Communities.Remove(entity);
        return true;
    }

    public async Task<bool> IfExists(Guid id)
    {
        return await context.Communities.AnyAsync(c => c.Id == id);
    }

    public IQueryable<Community> Queryable()
    {
        return context.Communities.AsQueryable();
    }
}
