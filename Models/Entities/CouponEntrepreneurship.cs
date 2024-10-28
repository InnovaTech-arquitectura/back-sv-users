using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class CouponEntrepreneurship
{
    public bool? Active { get; set; }

    public long Id { get; set; }

    public long IdCoupon { get; set; }

    public long IdEntrepreneurship { get; set; }

    public virtual Coupon IdCouponNavigation { get; set; } = null!;

    public virtual Entrepreneurship IdEntrepreneurshipNavigation { get; set; } = null!;
}
