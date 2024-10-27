using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_SV_users
{
    public class CouponEntrepreneurship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        // Relación con Entrepreneurship
        [Required]
        [ForeignKey("Entrepreneurship")]
        public long IdEntrepreneurship { get; set; }
        public Entrepreneurship Entrepreneurship { get; set; }

        // Relación con Coupon
        [Required]
        [ForeignKey("Coupon")]
        public long IdCoupon { get; set; }
        public Coupon Coupon { get; set; }

        // Campo de estado activo
        [Column("active")]
        public bool Active { get; set; } = true; // Valor por defecto true
    }
}
