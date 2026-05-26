using System;
using System.Collections.Generic;

namespace XClone.Domain.Database.SqlServer.Entities;

public partial class Chat
{
    public Guid Id { get; set; }

    public Guid UserLowId { get; set; }

    public Guid UserHighId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User UserLow { get; set; } = null!;

    public virtual User UserHigh { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
