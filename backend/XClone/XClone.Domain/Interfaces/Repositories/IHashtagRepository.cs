using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface IHashtagRepository
{
    Task<Hashtag> Create(Hashtag hashtag);
    Task<Hashtag?> GetByText(string text);
    Task<List<Hashtag>> GetTrending(int count);
    Task<bool> IfExists(string text);
    IQueryable<Hashtag> Queryable();
}
