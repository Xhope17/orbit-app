using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Auth
{
    public class LoginAuthRequest
    {
        [Required(ErrorMessage = ValidationConstants.REQUIRED)]
        [EmailAddress(ErrorMessage = ValidationConstants.INVALID_EMAIL)]
        public string email { get; set; } = null!;


    }
}
