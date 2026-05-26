using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Reply;

public class CreateReplyRequest
{
    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    public Guid PostId { get; set; }

    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    [MaxLength(280, ErrorMessage = ValidationConstants.MAX_LENGTH)]
    [MinLength(1, ErrorMessage = ValidationConstants.MIN_LENGTH)]
    public string Texto { get; set; } = null!;
}
