using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Quote;
using XClone.Application.Models.Responses;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.Application.Services;

public class QuoteService(IUnitOfWork uow, IUserService userService) : IQuoteService
{
    public async Task<GenericResponse<QuoteDto>> Create(CreateQuoteRequest model, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);

        if (!await uow.postRepository.IfExists(model.QuotedPostId))
            throw new NotFoundException(ResponseConstants.POST_NOT_EXIST);

        var quote = await uow.quoteRepository.Create(new Quote
        {
            QuotedPostId = model.QuotedPostId,
            AuthorId = executor.Id,
            Texto = model.Texto
        });
        await uow.SaveChangesAsync();

        return ResponseHelper.Create(new QuoteDto
        {
            Id = quote.Id,
            QuotedPostId = quote.QuotedPostId,
            Texto = quote.Texto,
            AuthorId = quote.AuthorId,
            AuthorName = executor.DisplayName,
            FechaCreacion = quote.FechaCreacion
        });
    }

    public async Task<GenericResponse<List<QuoteDto>>> GetByPost(Guid postId)
    {
        var quotes = await uow.quoteRepository.GetByPost(postId);
        var dtos = quotes.Select(q => new QuoteDto
        {
            Id = q.Id,
            QuotedPostId = q.QuotedPostId,
            Texto = q.Texto,
            AuthorId = q.AuthorId,
            AuthorName = q.Author.DisplayName,
            FechaCreacion = q.FechaCreacion
        }).ToList();
        return ResponseHelper.Create(dtos);
    }

    public async Task<GenericResponse<bool>> Delete(Guid id, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var quote = await uow.quoteRepository.Get(id)
            ?? throw new NotFoundException("Quote no encontrado");

        if (quote.AuthorId != executor.Id)
            throw new UnauthorizedException("No puedes eliminar un quote que no te pertenece");

        await uow.quoteRepository.Delete(id);
        await uow.SaveChangesAsync();
        return ResponseHelper.Create(true);
    }
}
