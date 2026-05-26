namespace XClone.Application.Models.DTOs;

public class CommunityMemberDto
{
    public Guid Id { get; set; }
    public Guid CommunityId { get; set; }
    public Guid UserId { get; set; }
    public string Role { get; set; } = null!;
    public UserDto? User { get; set; }
}
