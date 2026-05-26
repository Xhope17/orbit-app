using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Block;

public class BlockRequest
{
    [Required(ErrorMessage = ValidationConstants.REQUIRED)]
    public Guid BlockedUserId { get; set; }
}
