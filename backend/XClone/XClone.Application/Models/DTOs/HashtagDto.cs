namespace XClone.Application.Models.DTOs;

public class HashtagDto
{
    public Guid Id { get; set; }
    public string Texto { get; set; } = null!;
    public int PostCount { get; set; }
}
