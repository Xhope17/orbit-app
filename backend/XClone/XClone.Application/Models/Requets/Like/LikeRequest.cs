using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Like;

public class LikeRequest
{
    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    public Guid PostId { get; set; }
}
