using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_sv_users.Models.Entities
{
    [Table("plan", Schema = "public")] // Especifica el nombre de la tabla y el esquema
    public partial class Plan
    {
        [Key]
        [Column("id")] // Especifica el nombre de la columna en minúsculas
        public long Id { get; set; }

        [Column("price")]
        public double? Price { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
        public virtual ICollection<PlanFunctionality> PlanFunctionalities { get; set; } = new List<PlanFunctionality>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
