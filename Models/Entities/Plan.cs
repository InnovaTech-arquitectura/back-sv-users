using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class Plan
{
    public double? Price { get; set; }

    public long Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    public virtual ICollection<PlanFunctionality> PlanFunctionalities { get; set; } = new List<PlanFunctionality>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
