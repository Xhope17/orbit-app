using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Block;
using XClone.Application.Models.Responses;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.Application.Services;

public class BlockService(IUnitOfWork uow, IUserService userService) : IBlockService
{
    public async Task<GenericResponse<bool>> BlockUser(BlockRequest model, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (executor.Id == model.BlockedUserId)
            throw new BadRequestException("No puedes bloquearte a ti mismo");

        var target = await uow.userRepository.Get(model.BlockedUserId)
            ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST);

        if (await uow.blockRepository.IfExists(executor.Id, model.BlockedUserId))
            throw new BadRequestException("Ya has bloqueado a este usuario");

        await uow.blockRepository.Create(new()
        {
            BlockerId = executor.Id,
            BlockedId = model.BlockedUserId
        });
        await uow.SaveChangesAsync();

        return ResponseHelper.Create(true);
    }

    public async Task<GenericResponse<bool>> UnblockUser(Guid userId, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (!await uow.blockRepository.IfExists(executor.Id, userId))
            throw new NotFoundException("No has bloqueado a este usuario");

        await uow.blockRepository.Delete(executor.Id, userId);
        await uow.SaveChangesAsync();

        return ResponseHelper.Create(true);
    }

    public async Task<GenericResponse<List<UserDto>>> GetBlockedUsers(Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var blocked = await uow.blockRepository.GetBlockedByUser(executor.Id);
        var users = blocked.Select(b => new UserDto
        {
            Id = b.Blocked.Id,
            UserName = b.Blocked.UserName,
            DisplayName = b.Blocked.DisplayName
        }).ToList();
        return ResponseHelper.Create(users);
    }
}
