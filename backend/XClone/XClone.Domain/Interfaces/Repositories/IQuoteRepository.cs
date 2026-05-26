using XClone.Domain.Database.SqlServer.Entities;

namespace XClone.Domain.Interfaces.Repositories;

public interface IQuoteRepository
{
    Task<Quote> Create(Quote quote);
    Task<Quote?> Get(Guid id);
    Task<bool> Delete(Guid id);
    Task<List<Quote>> GetByPost(Guid postId);
    Task<bool> IfExists(Guid id);
    IQueryable<Quote> Queryable();
}
