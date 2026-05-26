using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface IPostHashtagRepository
{
    Task<PostHashtag> Create(PostHashtag postHashtag);
    Task<List<PostHashtag>> GetByPost(Guid postId);
    Task<List<PostHashtag>> GetByHashtag(Guid hashtagId);
    Task<bool> IfExists(Guid postId, Guid hashtagId);
    IQueryable<PostHashtag> Queryable();
}
