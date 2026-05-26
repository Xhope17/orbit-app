namespace XClone.Application.Models.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string? PhoneNumber { get; set; }

        public string? ProfilePictureUrl { get; set; }
        public string? BannerPictureUrl { get; set; }

        public bool IsVerified { get; set; }

        public Guid? PinnedPostId { get; set; }
        public Guid? TimezoneId { get; set; }
        public Guid? CityId { get; set; }

        public DateTime JoinedAt { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public RoleDto? Role { get; set; }
    }
}
