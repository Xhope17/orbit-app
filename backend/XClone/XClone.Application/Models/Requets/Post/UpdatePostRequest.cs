using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Post
{
    public class UpdatePostRequest
    {


        //[Required(ErrorMessage = ValidationConstants.REQUIRED)]      //las otras 2 son obligatorias antes que esta
        [MaxLength(250, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        [MinLength(1, ErrorMessage = ValidationConstants.MIN_LENGTH)]
        public string? Texto { get; set; }

        public bool? IsSensitive { get; set; }
    }
}
