namespace XClone.Application.Models.DTOs;

public class ChatDto
{
    public Guid Id { get; set; }
    public Guid UserLowId { get; set; }
    public Guid UserHighId { get; set; }
    public UserDto? UserLow { get; set; }
    public UserDto? UserHigh { get; set; }
    public DateTime CreatedAt { get; set; }
    public MessageDto? LastMessage { get; set; }
}
