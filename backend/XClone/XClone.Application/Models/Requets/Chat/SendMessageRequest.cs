using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Chat;

public class SendMessageRequest
{
    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    public Guid ReceiverId { get; set; }

    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    [MaxLength(1000, ErrorMessage = ValidationConstants.MAX_LENGTH)]
    [MinLength(1, ErrorMessage = ValidationConstants.MIN_LENGTH)]
    public string Texto { get; set; } = null!;
}
