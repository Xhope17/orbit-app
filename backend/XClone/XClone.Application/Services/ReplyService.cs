using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Reply;
using XClone.Application.Models.Responses;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.Application.Services;

public class ReplyService(IUnitOfWork uow, IUserService userService) : IReplyService
{
    public async Task<GenericResponse<ReplyDto>> Create(CreateReplyRequest model, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (!await uow.postRepository.IfExists(model.PostId))
            throw new NotFoundException(ResponseConstants.POST_NOT_EXIST);

        var reply = await uow.replyRepository.Create(new Reply
        {
            PostId = model.PostId,
            AuthorId = executor.Id,
            Texto = model.Texto
        });
        await uow.SaveChangesAsync();

        return ResponseHelper.Create(new ReplyDto
        {
            Id = reply.Id,
            PostId = reply.PostId,
            Texto = reply.Texto,
            AuthorId = reply.AuthorId,
            AuthorName = executor.DisplayName,
            FechaCreacion = reply.FechaCreacion
        });
    }

    public async Task<GenericResponse<List<ReplyDto>>> GetByPost(Guid postId)
    {
        var replies = await uow.replyRepository.GetByPost(postId);
        var dtos = replies.Select(r => new ReplyDto
        {
            Id = r.Id,
            PostId = r.PostId,
            Texto = r.Texto,
            AuthorId = r.AuthorId,
            AuthorName = r.Author.DisplayName,
            FechaCreacion = r.FechaCreacion
        }).ToList();
        return ResponseHelper.Create(dtos);
    }

    public async Task<GenericResponse<bool>> Delete(Guid id, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var reply = await uow.replyRepository.Get(id)
            ?? throw new NotFoundException("Respuesta no encontrada");

        if (reply.AuthorId != executor.Id)
            throw new UnauthorizedException("No puedes eliminar una respuesta que no te pertenece");

        await uow.replyRepository.Delete(id);
        await uow.SaveChangesAsync();
        return ResponseHelper.Create(true);
    }
}
