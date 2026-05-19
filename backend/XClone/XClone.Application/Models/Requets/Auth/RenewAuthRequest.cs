using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XClone.Application.Models.Requets.Auth
{
    public class RenewAuthRequest
    {
        [Required]
        [Description("Token que se usa para renovar la sesión. Este se consigue, al iniciar sesión en el aplicativo.")]
        public string RefreshToken { get; set; } = null!;
    }
}
