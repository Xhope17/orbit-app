namespace XClone.Application.Models.DTOs;

public class MediaFileDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = null!;
    public string PublicId { get; set; } = null!;
    public string Format { get; set; } = null!;
    public string ResourceType { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
