using System;
using System.Collections.Generic;

namespace XClone.Domain.Database.SqlServer.Entities;

public partial class Block
{
    public Guid Id { get; set; }

    public Guid BlockerId { get; set; }

    public Guid BlockedId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual User Blocked { get; set; } = null!;

    public virtual User Blocker { get; set; } = null!;
}
