using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class CommunityMemberRepository(XcloneContext context) : ICommunityMemberRepository
{
    public async Task<CommunityMember> Create(CommunityMember member)
    {
        await context.CommunityMembers.AddAsync(member);
        return member;
    }

    public async Task<bool> Delete(Guid communityId, Guid userId)
    {
        var entity = await context.CommunityMembers
            .FirstOrDefaultAsync(m => m.CommunityId == communityId && m.UserId == userId);
        if (entity is null) return false;
        context.CommunityMembers.Remove(entity);
        return true;
    }

    public async Task<CommunityMember?> Get(Guid communityId, Guid userId)
    {
        return await context.CommunityMembers
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.CommunityId == communityId && m.UserId == userId);
    }

    public async Task<List<CommunityMember>> GetByCommunity(Guid communityId)
    {
        return await context.CommunityMembers
            .Include(m => m.User)
            .Where(m => m.CommunityId == communityId)
            .ToListAsync();
    }

    public async Task<List<CommunityMember>> GetByUser(Guid userId)
    {
        return await context.CommunityMembers
            .Include(m => m.Community)
            .Where(m => m.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> IfExists(Guid communityId, Guid userId)
    {
        return await context.CommunityMembers
            .AnyAsync(m => m.CommunityId == communityId && m.UserId == userId);
    }

    public async Task<int> CountMembers(Guid communityId)
    {
        return await context.CommunityMembers.CountAsync(m => m.CommunityId == communityId);
    }

    public IQueryable<CommunityMember> Queryable()
    {
        return context.CommunityMembers.AsQueryable();
    }
}
