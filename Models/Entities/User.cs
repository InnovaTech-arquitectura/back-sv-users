using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class User
{
    public long Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
}
