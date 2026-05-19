using System;
using System.Collections.Generic;

namespace XClone.Domain.Database.SqlServer.Entities;

public partial class Repost
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public Guid UserId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
