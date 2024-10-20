using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class Functionality
{
    public long Id { get; set; }

    public string? Description { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<CouponFunctionality> CouponFunctionalities { get; set; } = new List<CouponFunctionality>();

    public virtual ICollection<PlanFunctionality> PlanFunctionalities { get; set; } = new List<PlanFunctionality>();
}
