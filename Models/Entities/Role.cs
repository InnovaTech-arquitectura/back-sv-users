using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class Role
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserEntity> UserEntities { get; set; } = new List<UserEntity>();
}
