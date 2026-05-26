using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Following;
using XClone.Application.Models.Responses;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.Application.Services;

public class FollowingService(IUnitOfWork uow, IUserService userService) : IFollowingService
{
    public async Task<GenericResponse<FollowingDto>> Follow(FollowRequest model, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (executor.Id == model.FollowedUserId)
            throw new BadRequestException("No puedes seguirte a ti mismo");

        var target = await uow.userRepository.Get(model.FollowedUserId)
            ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST);

        if (await uow.followingRepository.IfExists(executor.Id, model.FollowedUserId))
            throw new BadRequestException("Ya sigues a este usuario");

        var following = await uow.followingRepository.Create(new()
        {
            FollowerId = executor.Id,
            FollowedId = model.FollowedUserId
        });
        await uow.SaveChangesAsync();

        return ResponseHelper.Create(new FollowingDto
        {
            Id = following.Id,
            FollowerId = following.FollowerId,
            FollowedId = following.FollowedId
        });
    }

    public async Task<GenericResponse<bool>> Unfollow(Guid userId, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (!await uow.followingRepository.IfExists(executor.Id, userId))
            throw new NotFoundException("No sigues a este usuario");

        await uow.followingRepository.Delete(executor.Id, userId);
        await uow.SaveChangesAsync();

        return ResponseHelper.Create(true);
    }

    public async Task<GenericResponse<List<UserDto>>> GetFollowers(Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var followers = await uow.followingRepository.GetFollowers(executor.Id);
        var users = followers.Select(f => MapUser(f.Follower)).ToList();
        return ResponseHelper.Create(users);
    }

    public async Task<GenericResponse<List<UserDto>>> GetFollowing(Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var following = await uow.followingRepository.GetFollowed(executor.Id);
        var users = following.Select(f => MapUser(f.Followed)).ToList();
        return ResponseHelper.Create(users);
    }

    public async Task<GenericResponse<object>> GetCounts(Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var followers = await uow.followingRepository.CountFollowers(executor.Id);
        var followed = await uow.followingRepository.CountFollowed(executor.Id);
        return ResponseHelper.Create<object>(new { followers, following = followed });
    }

    private static UserDto MapUser(Domain.Database.SqlServer.Entities.User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Age = user.Age,
            PhoneNumber = user.PhoneNumber,
            IsVerified = user.IsVerified,
            ProfilePictureUrl = user.ProfilePictureUrl,
            BannerPictureUrl = user.BannerPictureUrl,
            JoinedAt = user.JoinedAt,
            IsActive = user.IsActive
        };
    }
}
