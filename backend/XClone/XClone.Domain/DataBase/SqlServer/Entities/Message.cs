using System;
using System.Collections.Generic;

namespace XClone.Domain.Database.SqlServer.Entities;

public partial class Message
{
    public Guid Id { get; set; }

    public string Texto { get; set; } = null!;

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public Guid? PostId { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User Sender { get; set; } = null!;
}
