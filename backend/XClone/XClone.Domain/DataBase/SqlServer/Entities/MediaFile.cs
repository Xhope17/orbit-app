using System;
using System.Collections.Generic;

namespace XClone.Domain.Database.SqlServer.Entities;

public partial class MediaFile
{
    public Guid Id { get; set; }

    public string Url { get; set; } = null!;

    public string PublicId { get; set; } = null!;

    public string Format { get; set; } = null!;

    public string ResourceType { get; set; } = null!;

    public Guid? UserId { get; set; }

    public Guid? PostId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? User { get; set; }

    public virtual Post? Post { get; set; }
}
