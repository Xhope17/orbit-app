using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface ICommunityRepository
{
    Task<Community> Create(Community community);
    Task<Community?> Get(Guid id);
    Task<Community> Update(Community community);
    Task<bool> Delete(Guid id);
    Task<bool> IfExists(Guid id);
    IQueryable<Community> Queryable();
}
