using System.ComponentModel.DataAnnotations;
using XClone.Shared.Constants;

namespace XClone.Application.Models.Requets.Post
{
    public class CreatePostRequest
    {

        //[Required(ErrorMessage = ValidationConstants.REQUIRED)]
        public Guid AuthorId { get; set; }

        [MaxLength(255, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        //[MinLength(10, ErrorMessage = ValidationConstants.MIN_LENGTH)]
        public Guid? CommunityId { get; set; }

        [Required(ErrorMessage = ValidationConstants.REQUIRED)]
        [MaxLength(280, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        [MinLength(1, ErrorMessage = ValidationConstants.MIN_LENGTH)]
        public string Texto { get; set; } = null!;
        //= null!;

        public bool IsSensitive { get; set; } = false;
    }
}
