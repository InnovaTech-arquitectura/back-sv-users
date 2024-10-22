using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class Client
{
    public int Id { get; set; }

    public long? IdUser { get; set; }

    public string? IdCard { get; set; }
}
