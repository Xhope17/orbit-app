using System;
using System.Collections.Generic;

namespace XClone.Domain.Database.SqlServer.Entities;

public partial class PostHashtag
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public Guid HashtagId { get; set; }

    public virtual Hashtag Hashtag { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
