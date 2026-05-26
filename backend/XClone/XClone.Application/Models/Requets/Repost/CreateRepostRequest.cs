using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Repost;

public class CreateRepostRequest
{
    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    public Guid PostId { get; set; }
}
