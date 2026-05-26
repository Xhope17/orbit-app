using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface ICommunityMemberRepository
{
    Task<CommunityMember> Create(CommunityMember member);
    Task<bool> Delete(Guid communityId, Guid userId);
    Task<CommunityMember?> Get(Guid communityId, Guid userId);
    Task<List<CommunityMember>> GetByCommunity(Guid communityId);
    Task<List<CommunityMember>> GetByUser(Guid userId);
    Task<bool> IfExists(Guid communityId, Guid userId);
    Task<int> CountMembers(Guid communityId);
    IQueryable<CommunityMember> Queryable();
}
