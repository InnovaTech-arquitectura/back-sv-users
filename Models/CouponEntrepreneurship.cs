using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_SV_users
{
    public class CouponEntrepreneurship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long id { get; set; }

        [ForeignKey("Entrepreneurship")]
        [Column("id_entrepreneurship")]
        public long IdEntrepreneurship { get; set; } // Tipo compatible con el Id de Entrepreneurship
        public Entrepreneurship Entrepreneurship { get; set; }

        [ForeignKey("Coupon")]
        [Column("id_coupon")]
        public long IdCoupon { get; set; }
        public Coupon Coupon { get; set; }


        [Column("active")]
        public bool Active { get; set; } = true; // Valor predeterminado
    }
}
