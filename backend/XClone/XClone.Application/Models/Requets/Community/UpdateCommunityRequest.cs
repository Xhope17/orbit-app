using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Community;

public class UpdateCommunityRequest
{
    [MaxLength(100, ErrorMessage = ValidationConstants.MAX_LENGTH)]
    public string? CommunityName { get; set; }

    [MaxLength(255, ErrorMessage = ValidationConstants.MAX_LENGTH)]
    public string? Description { get; set; }
}
