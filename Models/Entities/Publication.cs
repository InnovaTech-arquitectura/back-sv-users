using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class Publication
{
    public long? AdministrativeEmployeeId { get; set; }

    public long Id { get; set; }

    public string Multimedia { get; set; } = null!;

    public string Title { get; set; } = null!;

    public virtual AdministrativeEmployee? AdministrativeEmployee { get; set; }
}
