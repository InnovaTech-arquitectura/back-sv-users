using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_SV_users
{
    public class Entrepreneurship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }  // Cambiado de int a long

        [Required]
        [Column("name")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Column("logo")]
        public string? Logo { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("user_entity_id")]
        public long? UserEntityId { get; set; }  // Agregado para mapear el ID de usuario

        public List<CouponEntrepreneurship> CouponEntrepreneurships { get; set; } = new List<CouponEntrepreneurship>();
    }
}
