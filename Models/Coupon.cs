using back_sv_users.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_SV_users
{
    public class Coupon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("description")]
        [MaxLength(500)]
        public string? Description { get; set; }

        [Column("expiration_date")]
        public DateTime? ExpirationDate { get; set; }

        [Column("expiration_period")]
        public int? ExpirationPeriod { get; set; }

        [ForeignKey("Entrepreneurship")]
        [Column("id_entrepreneurship")]
        public long IdEntrepreneurship { get; set; }
        public Entrepreneurship Entrepreneurship { get; set; }

        [ForeignKey("Plan")]
        [Column("id_plan")]
        public long? IdPlan { get; set; }
        public Plan Plan { get; set; }

        public List<CouponFunctionality> CouponFunctionalities { get; set; } = new List<CouponFunctionality>();

        // Agregar la propiedad de navegación para CouponEntrepreneurships
        public List<CouponEntrepreneurship> CouponEntrepreneurships { get; set; } = new List<CouponEntrepreneurship>();
    }
}
