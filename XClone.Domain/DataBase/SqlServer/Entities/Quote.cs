using System;
using System.Collections.Generic;

namespace XClone.Domain.Database.SqlServer.Entities;

public partial class Quote
{
    public Guid Id { get; set; }

    public Guid QuotedPostId { get; set; }

    public string Texto { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual Post QuotedPost { get; set; } = null!;
}
