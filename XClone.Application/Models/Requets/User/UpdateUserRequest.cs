using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.User
{
    public class UpdateUserRequest
    {
        [MaxLength(250, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        [MinLength(1, ErrorMessage = ValidationConstants.MIN_LENGTH)]
        public string? UserName { get; set; }

        [MaxLength(100, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        public string? DisplayName { get; set; }

        [Range(18, 120, ErrorMessage = ValidationConstants.INVALID_AGE)]
        public int? Age { get; set; }

        [MaxLength(250, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        public string? PhoneNumber { get; set; }
    }
}
