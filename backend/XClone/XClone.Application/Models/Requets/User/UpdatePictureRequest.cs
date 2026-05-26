using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.User
{
    public class UpdatePictureRequest
    {
        [Required(ErrorMessage = ValidationConstants.REQUIRED)]
        [MaxLength(500, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        public string Url { get; set; } = null!;
    }
}
