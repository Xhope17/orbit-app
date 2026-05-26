using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Following;

public class FollowRequest
{
    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    public Guid FollowedUserId { get; set; }
}
