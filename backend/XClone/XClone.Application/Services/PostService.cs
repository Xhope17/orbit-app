using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Post;
using XClone.Application.Models.Responses;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Exceptions;
using XClone.Domain.Interfaces.Repositories;
using XClone.Shared.Constants;
using XClone.Shared.Helpers;

namespace XClone.Application.Services
{
    public class PostService(IPostRepository repository) : IPostService

    {
        //Crear post
        public async Task<GenericResponse<PostDto>> Create(CreatePostRequest model)
        {
            var create = await repository.Create(new Post
            {
                AuthorId = model.AuthorId,
                CommunityId = model.CommunityId,
                Texto = model.Texto,
                IsSensitive = model.IsSensitive
            });



            return ResponseHelper.Create(Map(create));
        }

        //borrar post
        public async Task<GenericResponse<bool>> Delete(Guid postId)
        {
            var post = await GetPost(postId);

            post.DeletedAt = DateTimeHelper.UtcNow();
            post.IsActive = false;
            await repository.Update(post);

            return ResponseHelper.Create(true);
        }

        //obtener todos los post
        //public GenericResponse<List<PostDto>> Get(int limit, int offset)
        public GenericResponse<List<PostDto>> Get(FilterPostRequest model)
        {
            var queryble = repository.Queryable();

            if (!string.IsNullOrWhiteSpace(model.Texto))
            {
                queryble = queryble.Where(x => x.Texto != null && x.Texto.Contains(model.Texto ?? ""));

            }

            //realizar paginacion y consulta
            var posts = queryble.Skip(model.Offset).Take(model.Limit).ToList();

            //Mapper psot
            List<PostDto> mapped = [];
            foreach (var post in posts)
            {
                mapped.Add(Map(post));
            }

            return ResponseHelper.Create(mapped);
        }

        //obtener un post por id
        public async Task<GenericResponse<PostDto>> Get(Guid postId)
        {
            var post = await GetPost(postId);

            return ResponseHelper.Create(Map(post));

        }

        //editar un post
        public async Task<GenericResponse<PostDto>> Update(Guid postId, UpdatePostRequest model)
        {
            var post = await GetPost(postId);

            post.Texto = model.Texto ?? post.Texto;
            post.IsSensitive = model.IsSensitive ?? post.IsSensitive;

            var update = await repository.Update(post);

            return ResponseHelper.Create(Map(update));

        }

        private async Task<Post> GetPost(Guid postId)
        {
            return await repository.Get(postId)
                ?? throw new NotFoundException(ResponseConstants.POST_NOT_EXIST);
        }

        private PostDto Map(Post post)
        {
            return new PostDto
            {

                Id = post.Id,
                AuthorId = post.AuthorId,
                Texto = post.Texto,
                IsSensitive = post.IsSensitive,
                CommunityId = post.CommunityId,
                JoinedAt = post.JoinedAt,
                IsActive = post.IsActive,
                CreateAt = post.CreateAt,
                UpdatedAt = post.UpdatedAt

            };
        }
    }
}
