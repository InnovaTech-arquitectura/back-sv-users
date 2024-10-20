using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class UserEntity
{
    public int IdCard { get; set; }

    public long? AdministrativeEmployeeId { get; set; }

    public long Id { get; set; }

    public long RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public string Password { get; set; } = null!;

    public virtual AdministrativeEmployee? AdministrativeEmployee { get; set; }

    public virtual AdministrativeEmployee? AdministrativeEmployeeNavigation { get; set; }

    public virtual Role Role { get; set; } = null!;
}
