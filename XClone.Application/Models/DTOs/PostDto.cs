namespace XClone.Application.Models.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Texto { get; set; } = null!;
        public bool IsSensitive { get; set; }
        public Guid? CommunityId { get; set; }
        public DateTime JoinedAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; } //
    }
}
