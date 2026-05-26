using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Community;

public class CreateCommunityRequest
{
    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    [MaxLength(100, ErrorMessage = ValidationConstants.MAX_LENGTH)]
    [MinLength(3, ErrorMessage = ValidationConstants.MIN_LENGTH)]
    public string CommunityName { get; set; } = null!;

    [MaxLength(255, ErrorMessage = ValidationConstants.MAX_LENGTH)]
    public string? Description { get; set; }
}
