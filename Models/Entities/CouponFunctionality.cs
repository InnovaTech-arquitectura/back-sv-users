using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace back_sv_users.Models.Entities;

public partial class CouponFunctionality
{

    [Key]
    [Column("id")]
    public long Id { get; set; }



 
    [Column("id_coupon ")]   
    public long IdCoupon { get; set; }

    [Column("id_functionality")]   
    public long IdFunctionality { get; set; }

    public virtual Coupon IdCouponNavigation { get; set; } = null!;

    public virtual Functionality IdFunctionalityNavigation { get; set; } = null!;
}
