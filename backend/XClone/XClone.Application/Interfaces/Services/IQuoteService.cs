using System.Security.Claims;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Quote;
using XClone.Application.Models.Responses;

namespace XClone.Application.Interfaces.Services;

public interface IQuoteService
{
    Task<GenericResponse<QuoteDto>> Create(CreateQuoteRequest model, Claim claim);
    Task<GenericResponse<List<QuoteDto>>> GetByPost(Guid postId);
    Task<GenericResponse<bool>> Delete(Guid id, Claim claim);
}
