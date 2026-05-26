namespace XClone.Application.Models.DTOs;

public class CommunityDto
{
    public Guid Id { get; set; }
    public string CommunityName { get; set; } = null!;
    public string? Description { get; set; }
    public Guid CreatorId { get; set; }
    public UserDto? Creator { get; set; }
    public int MemberCount { get; set; }
    public string? MemberRole { get; set; }
}
