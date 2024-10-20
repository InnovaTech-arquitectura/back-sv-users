using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class CouponFunctionality
{
    public long Id { get; set; }

    public long IdCoupon { get; set; }

    public long IdFunctionality { get; set; }

    public virtual Coupon IdCouponNavigation { get; set; } = null!;

    public virtual Functionality IdFunctionalityNavigation { get; set; } = null!;
}
