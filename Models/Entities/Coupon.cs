using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class Coupon
{
    public int? ExpirationPeriod { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public long Id { get; set; }

    public long IdEntrepreneurship { get; set; }

    public long? IdPlan { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CouponEntrepreneurship> CouponEntrepreneurships { get; set; } = new List<CouponEntrepreneurship>();

    public virtual ICollection<CouponFunctionality> CouponFunctionalities { get; set; } = new List<CouponFunctionality>();

    public virtual Entrepreneurship IdEntrepreneurshipNavigation { get; set; } = null!;

    public virtual Plan? IdPlanNavigation { get; set; }
}
