using Microsoft.EntityFrameworkCore;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer.Repositories
{
    public class PostRepository(XcloneContext context) : IPostRepository
    {
        public async Task<Post> Create(Post post)
        {
            await context.Posts.AddAsync(post);
            return post;
        }

        public async Task<Post?> Get(Guid postId)
        {
            return await context.Posts.FirstOrDefaultAsync(x => x.Id == postId && x.IsActive == true);
        }
        public async Task<bool> IfExists(Guid postId)
        {
            try
            {
                return await context.Posts.AnyAsync(x => x.Id == postId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<Post> Queryable()
        {
            return context.Posts.Where(x => x.IsActive == true).AsQueryable();
        }

        public async Task<Post> Update(Post post)
        {
            context.Posts.Update(post);
            return post;
        }
    }
}
