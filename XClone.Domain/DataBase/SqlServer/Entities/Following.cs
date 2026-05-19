using System;
using System.Collections.Generic;

namespace XClone.Domain.Database.SqlServer.Entities;

public partial class Following
{
    public Guid Id { get; set; }

    public Guid FollowerId { get; set; }

    public Guid FollowedId { get; set; }

    public virtual User Followed { get; set; } = null!;

    public virtual User Follower { get; set; } = null!;
}
