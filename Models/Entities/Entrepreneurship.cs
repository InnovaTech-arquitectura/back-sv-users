using System;
using System.Collections.Generic;
using back_sv_Users.Models.Entities;

namespace back_sv_users.Models.Entities;

public partial class Entrepreneurship
{
    public long Id { get; set; }

    public string? Description { get; set; }

    public string? Lastnames { get; set; }

    public string? Logo { get; set; }

    public string Name { get; set; } = null!;

    public string? Names { get; set; }

    public virtual ICollection<CouponEntrepreneurship> CouponEntrepreneurships { get; set; } = new List<CouponEntrepreneurship>();

    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    public virtual ICollection<CourseEntrepreneurship> CourseEntrepreneurships { get; set; } = new List<CourseEntrepreneurship>();

    public virtual ICollection<EntrepreneurshipEventRegistry> EntrepreneurshipEventRegistries { get; set; } = new List<EntrepreneurshipEventRegistry>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
