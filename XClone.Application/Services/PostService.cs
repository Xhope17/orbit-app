using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Post;
using XClone.Application.Models.Responses;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Shared;
using XClone.Shared.Constants;
using XClone.Shared.Helpers;

namespace XClone.Application.Services
{
    public class PostService(Cache<PostDto> cache, XcloneContext xcloneContext) : IPostService
    {
        //Crear post
        public GenericResponse<PostDto> Create(CreatePostRequest model)
        {
            var post = new PostDto
            {
                PostId = Guid.NewGuid(),
                AutorId = model.AutorId,
                Comunidad = model.Comunidad,
                Texto = model.Texto,
                CreatedAt = DateTimeHelper.UtcNow(),
                JoinedAt = DateTimeHelper.UtcNow()
            };
            cache.Add(post.PostId.ToString(), post);

            return ResponseHelper.Create(post, "Post subido correctamente");
        }

        //borrar post
        public GenericResponse<bool> Delete(Guid postId)
        {
            var isDeleted = cache.Get(postId.ToString());

            if (isDeleted is null)
            {
                return ResponseHelper.Create(false);
            }

            cache.Delete(postId.ToString());

            return ResponseHelper.Create(true, "Post eliminado");
        }

        //obtener todos los post
        public GenericResponse<List<PostDto>> Get(int limit, int offset)
        {
            var posts = cache.Get();
            return ResponseHelper.Create(posts);
        }

        //obtener un post por id
        public GenericResponse<PostDto?> Get(Guid postId)
        {
            var post = cache.Get(postId.ToString());

            return ResponseHelper.Create(post, "Usuario encontrado");
        }

        //editar un post
        public GenericResponse<PostDto> Update(Guid postId, UpdatePostRequest model)
        {
            var exist = cache.Get(postId.ToString());

            if (exist is null)
            {
                return ResponseHelper.Create<PostDto>(null!, ValidationConstants.POST_NOT_FOUND);
            }

            exist.AutorId = model.AutorId;
            exist.Comunidad = model.Comunidad;
            exist.Texto = model.Texto;

            cache.Update(postId.ToString(), exist);

            return ResponseHelper.Create(exist, "Post actualizado");
        }

    }
}
