using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Auth.RecoverPassword
{
    public class RecoverPasswordSendOTPAuthRequest
    {
        [Required(ErrorMessage = ValidationConstants.REQUIRED)]
        [EmailAddress(ErrorMessage = ValidationConstants.EMAIL_ADDRESS)]
        [MaxLength(255, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        [MinLength(10, ErrorMessage = ValidationConstants.MIN_LENGTH)]
        [Description("El correo electrónico del usuario, para envíarle el código OTP")]
        public string Email { get; set; } = null!;
    }
}
