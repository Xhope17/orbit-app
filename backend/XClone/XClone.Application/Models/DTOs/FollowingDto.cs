namespace XClone.Application.Models.DTOs;

public class FollowingDto
{
    public Guid Id { get; set; }
    public Guid FollowerId { get; set; }
    public Guid FollowedId { get; set; }
    public UserDto? Follower { get; set; }
    public UserDto? Followed { get; set; }
}
