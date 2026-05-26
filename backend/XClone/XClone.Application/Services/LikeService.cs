using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Like;
using XClone.Application.Models.Responses;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.Application.Services;

public class LikeService(IUnitOfWork uow, IUserService userService) : ILikeService
{
    public async Task<GenericResponse<bool>> Like(LikeRequest model, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (!await uow.postRepository.IfExists(model.PostId))
            throw new NotFoundException(ResponseConstants.POST_NOT_EXIST);

        if (await uow.likeRepository.IfExists(model.PostId, executor.Id))
            throw new BadRequestException("Ya has dado like a este post");

        await uow.likeRepository.Create(new()
        {
            PostId = model.PostId,
            UserId = executor.Id
        });
        await uow.SaveChangesAsync();
        return ResponseHelper.Create(true);
    }

    public async Task<GenericResponse<bool>> Unlike(Guid postId, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (!await uow.likeRepository.IfExists(postId, executor.Id))
            throw new NotFoundException("No has dado like a este post");

        await uow.likeRepository.Delete(postId, executor.Id);
        await uow.SaveChangesAsync();
        return ResponseHelper.Create(true);
    }

    public async Task<GenericResponse<int>> CountByPost(Guid postId)
    {
        var count = await uow.likeRepository.CountByPost(postId);
        return ResponseHelper.Create(count);
    }
}
