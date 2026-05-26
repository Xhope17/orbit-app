namespace XClone.Application.Models.DTOs;

public class ReplyDto
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public string Texto { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public DateTime FechaCreacion { get; set; }
}
