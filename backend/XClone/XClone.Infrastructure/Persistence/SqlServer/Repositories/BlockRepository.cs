using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories;

public class BlockRepository(XcloneContext context) : IBlockRepository
{
    public async Task<Block> Create(Block block)
    {
        await context.Blocks.AddAsync(block);
        return block;
    }

    public async Task<bool> Delete(Guid blockerId, Guid blockedId)
    {
        var entity = await context.Blocks
            .FirstOrDefaultAsync(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
        if (entity is null) return false;
        context.Blocks.Remove(entity);
        return true;
    }

    public async Task<Block?> Get(Guid blockerId, Guid blockedId)
    {
        return await context.Blocks
            .FirstOrDefaultAsync(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
    }

    public async Task<List<Block>> GetBlockedByUser(Guid userId)
    {
        return await context.Blocks
            .Include(b => b.Blocked)
            .Where(b => b.BlockerId == userId)
            .ToListAsync();
    }

    public async Task<bool> IfExists(Guid blockerId, Guid blockedId)
    {
        return await context.Blocks
            .AnyAsync(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
    }

    public IQueryable<Block> Queryable()
    {
        return context.Blocks.AsQueryable();
    }
}
