using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class AdministrativeEmployee
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();

    public virtual UserEntity User { get; set; } = null!;

    public virtual UserEntity? UserEntity { get; set; }
}
