using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Auth.RecoverPassword
{
    public class RecoverPasswordAuthRequest
    {
        [Required(ErrorMessage = ValidationConstants.REQUIRED)]
        [MaxLength(255, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        [MinLength(10, ErrorMessage = ValidationConstants.MIN_LENGTH)]
        [Description("La nueva contraseña que quiere establecer el usuario")]
        public string NewPassword { get; set; } = null!;
    }
}
