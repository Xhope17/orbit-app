using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Responses;
using XClone.Domain.DataBase.SqlServer;

namespace XClone.Application.Services;

public class FeedService(IUnitOfWork uow, IUserService userService) : IFeedService
{
    public async Task<GenericResponse<List<PostDto>>> GetFeed(Claim claim, int limit, int offset)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var followed = await uow.followingRepository.GetFollowed(executor.Id);
        var followedIds = followed.Select(f => f.FollowedId).ToList();
        followedIds.Add(executor.Id);

        var posts = uow.postRepository.Queryable()
            .Where(p => followedIds.Contains(p.AuthorId))
            .OrderByDescending(p => p.CreateAt)
            .Skip(offset)
            .Take(limit)
            .ToList();

        var dtos = posts.Select(p => new PostDto
        {
            Id = p.Id,
            AuthorId = p.AuthorId,
            Texto = p.Texto,
            IsSensitive = p.IsSensitive,
            MediaUrl = p.MediaUrl,
            Visibility = p.Visibility,
            CommunityId = p.CommunityId,
            JoinedAt = p.JoinedAt,
            IsActive = p.IsActive,
            CreateAt = p.CreateAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();

        return ResponseHelper.Create(dtos);
    }
}
