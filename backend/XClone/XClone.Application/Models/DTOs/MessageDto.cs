namespace XClone.Application.Models.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string Texto { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
