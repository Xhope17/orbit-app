using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Quote;

public class CreateQuoteRequest
{
    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    public Guid QuotedPostId { get; set; }

    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    [MaxLength(280, ErrorMessage = ValidationConstants.MAX_LENGTH)]
    [MinLength(1, ErrorMessage = ValidationConstants.MIN_LENGTH)]
    public string Texto { get; set; } = null!;
}
