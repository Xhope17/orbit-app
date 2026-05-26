using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface IBlockRepository
{
    Task<Block> Create(Block block);
    Task<bool> Delete(Guid blockerId, Guid blockedId);
    Task<Block?> Get(Guid blockerId, Guid blockedId);
    Task<List<Block>> GetBlockedByUser(Guid userId);
    Task<bool> IfExists(Guid blockerId, Guid blockedId);
    IQueryable<Block> Queryable();
}
