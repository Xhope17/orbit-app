using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.Requets.Repost;
using XClone.Application.Models.Responses;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.Application.Services;

public class RepostService(IUnitOfWork uow, IUserService userService) : IRepostService
{
    public async Task<GenericResponse<bool>> Repost(CreateRepostRequest model, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (!await uow.postRepository.IfExists(model.PostId))
            throw new NotFoundException(ResponseConstants.POST_NOT_EXIST);

        if (await uow.repostRepository.IfExists(model.PostId, executor.Id))
            throw new BadRequestException("Ya has reposteado este post");

        await uow.repostRepository.Create(new()
        {
            PostId = model.PostId,
            UserId = executor.Id
        });
        await uow.SaveChangesAsync();
        return ResponseHelper.Create(true);
    }

    public async Task<GenericResponse<bool>> Unrepost(Guid postId, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (!await uow.repostRepository.IfExists(postId, executor.Id))
            throw new NotFoundException("No has reposteado este post");

        await uow.repostRepository.Delete(postId, executor.Id);
        await uow.SaveChangesAsync();
        return ResponseHelper.Create(true);
    }

    public async Task<GenericResponse<int>> CountByPost(Guid postId)
    {
        var count = await uow.repostRepository.CountByPost(postId);
        return ResponseHelper.Create(count);
    }
}
