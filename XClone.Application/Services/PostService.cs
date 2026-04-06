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
    //Guardado en cache
    //public class PostService(Cache<PostDto> cache, XcloneContext xcloneContext) : IPostService
    public class PostService(IPostRepository repository) : IPostService

    {
        //Crear post
        public async Task<GenericResponse<PostDto>> Create(CreatePostRequest model)
        {

            /*Guardado cache
            //var post = new PostDto
            //{
            //    PostId = Guid.NewGuid(),
            //    AutorId = model.AutorId,
            //    Comunidad = model.Comunidad,
            //    Texto = model.Texto,
            //    CreatedAt = DateTimeHelper.UtcNow(),
            //    JoinedAt = DateTimeHelper.UtcNow()
            //};
            //cache.Add(post.PostId.ToString(), post);
            */

            var create = await repository.Create(new Post
            {
                //PostId = Guid.NewGuid(),
                //AuthorId = Guid.Parse(model.AutorId),
                AuthorId = model.AuthorId,
                CommunityId = model.CommunityId,
                Texto = model.Texto,
                IsSensitive = model.IsSensitive

                //CommunityId = post.CommunityId,

            });



            return ResponseHelper.Create(Map(create));
        }

        //borrar post
        public async Task<GenericResponse<bool>> Delete(Guid postId)
        {
            /*por cache
            //var isDeleted = cache.Get(postId.ToString());

            //if (isDeleted is null)
            //{
            //    return ResponseHelper.Create(false);
            //}

            //cache.Delete(postId.ToString());

            //return ResponseHelper.Create(true, "Post eliminado");
            */

            //var fPost = repository.Get(post) ?? throw new Exception("Post no encontrado");
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
            /*por cache
            //var posts = cache.Get();
            //return ResponseHelper.Create(posts);
            */

            // Filtrado de texto
            var queryble = repository.Queryable();

            if (!string.IsNullOrWhiteSpace(model.Texto))
            {
                //queryble = queryble.Where(x => x.Author == model.Author);
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
            //var post = cache.Get(postId.ToString());
            //return ResponseHelper.Create(post, "Usuario encontrado");
            var post = await GetPost(postId);

            return ResponseHelper.Create(Map(post));

        }

        //editar un post
        public async Task<GenericResponse<PostDto>> Update(Guid postId, UpdatePostRequest model)
        {
            /*por cache
            //var exist = cache.Get(postId.ToString());

            //if (exist is null)
            //{
            //    return ResponseHelper.Create<PostDto>(null!, ValidationConstants.POST_NOT_FOUND);
            //}

            //exist.AutorId = model.AutorId;
            //exist.Comunidad = model.Comunidad;
            //exist.Texto = model.Texto;

            //cache.Update(postId.ToString(), exist);

            //return ResponseHelper.Create(exist, "Post actualizado");
            */
            var post = await GetPost(postId);

            post.Texto = model.Texto ?? post.Texto;
            post.IsSensitive = model.IsSensitive ?? post.IsSensitive;

            var update = await repository.Update(post);

            return ResponseHelper.Create(Map(update));

        }

        private async Task<Post> GetPost(Guid postId)
        {
            return await repository.Get(postId)
                //?? throw new Exception("Post no encontrado");
                ?? throw new NotFoundException(ResponseConstans.POST_NOT_EXIST);
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
