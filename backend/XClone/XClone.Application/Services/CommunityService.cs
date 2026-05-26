using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Community;
using XClone.Application.Models.Responses;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.Application.Services;

public class CommunityService(IUnitOfWork uow, IUserService userService) : ICommunityService
{
    public async Task<GenericResponse<CommunityDto>> Create(CreateCommunityRequest model, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        var community = await uow.communityRepository.Create(new Community
        {
            CommunityName = model.CommunityName,
            Description = model.Description,
            CreatorId = executor.Id
        });

        await uow.communityMemberRepository.Create(new CommunityMember
        {
            CommunityId = community.Id,
            UserId = executor.Id,
            Role = "Owner"
        });

        await uow.SaveChangesAsync();
        return ResponseHelper.Create(await MapCommunity(community, executor.Id));
    }

    public async Task<GenericResponse<CommunityDto>> Get(Guid id)
    {
        var community = await uow.communityRepository.Get(id)
            ?? throw new NotFoundException("Comunidad no encontrada");
        return ResponseHelper.Create(await MapCommunity(community, null));
    }

    public async Task<GenericResponse<List<CommunityDto>>> GetAll()
    {
        var communities = await Task.Run(() => uow.communityRepository.Queryable().ToList());
        var dtos = new List<CommunityDto>();
        foreach (var c in communities)
            dtos.Add(await MapCommunity(c, null));
        return ResponseHelper.Create(dtos);
    }

    public async Task<GenericResponse<CommunityDto>> Update(Guid id, UpdateCommunityRequest model, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var community = await uow.communityRepository.Get(id)
            ?? throw new NotFoundException("Comunidad no encontrada");

        if (community.CreatorId != executor.Id)
            throw new UnauthorizedException("Solo el creador puede editar la comunidad");

        community.CommunityName = model.CommunityName ?? community.CommunityName;
        community.Description = model.Description ?? community.Description;

        await uow.communityRepository.Update(community);
        await uow.SaveChangesAsync();
        return ResponseHelper.Create(await MapCommunity(community, executor.Id));
    }

    public async Task<GenericResponse<bool>> Delete(Guid id, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var community = await uow.communityRepository.Get(id)
            ?? throw new NotFoundException("Comunidad no encontrada");

        if (community.CreatorId != executor.Id)
            throw new UnauthorizedException("Solo el creador puede eliminar la comunidad");

        await uow.communityRepository.Delete(id);
        await uow.SaveChangesAsync();
        return ResponseHelper.Create(true);
    }

    public async Task<GenericResponse<bool>> Join(Guid communityId, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (await uow.communityMemberRepository.IfExists(communityId, executor.Id))
            throw new BadRequestException("Ya eres miembro de esta comunidad");

        await uow.communityMemberRepository.Create(new CommunityMember
        {
            CommunityId = communityId,
            UserId = executor.Id,
            Role = "Member"
        });
        await uow.SaveChangesAsync();
        return ResponseHelper.Create(true);
    }

    public async Task<GenericResponse<bool>> Leave(Guid communityId, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (!await uow.communityMemberRepository.IfExists(communityId, executor.Id))
            throw new NotFoundException("No eres miembro de esta comunidad");

        await uow.communityMemberRepository.Delete(communityId, executor.Id);
        await uow.SaveChangesAsync();
        return ResponseHelper.Create(true);
    }

    public async Task<GenericResponse<List<CommunityMemberDto>>> GetMembers(Guid communityId)
    {
        var members = await uow.communityMemberRepository.GetByCommunity(communityId);
        var dtos = members.Select(m => new CommunityMemberDto
        {
            Id = m.Id,
            CommunityId = m.CommunityId,
            UserId = m.UserId,
            Role = m.Role,
            User = new UserDto { Id = m.User.Id, UserName = m.User.UserName, DisplayName = m.User.DisplayName }
        }).ToList();
        return ResponseHelper.Create(dtos);
    }

    private async Task<CommunityDto> MapCommunity(Community community, Guid? currentUserId)
    {
        var memberCount = await uow.communityMemberRepository.CountMembers(community.Id);
        string? memberRole = null;

        if (currentUserId.HasValue)
        {
            var member = await uow.communityMemberRepository.Get(community.Id, currentUserId.Value);
            memberRole = member?.Role;
        }

        return new CommunityDto
        {
            Id = community.Id,
            CommunityName = community.CommunityName,
            Description = community.Description,
            CreatorId = community.CreatorId,
            MemberCount = memberCount,
            MemberRole = memberRole
        };
    }
}
