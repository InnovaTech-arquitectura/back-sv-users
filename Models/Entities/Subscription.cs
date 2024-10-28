using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_sv_users.Models.Entities
{
    [Table("subscription", Schema = "public")] // Especifica el nombre de la tabla y el esquema
    public partial class Subscription
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        [Column("expiration_date")]
        public DateOnly? ExpirationDate { get; set; }

        [Column("initial_date")]
        public DateOnly? InitialDate { get; set; }

        [Column("entrepreneurship_id")]
        public long? EntrepreneurshipId { get; set; }

        [Column("id_plan")]
        public long IdPlan { get; set; }

        public virtual Entrepreneurship? Entrepreneurship { get; set; }

        [ForeignKey("IdPlan")]
        public virtual Plan IdPlanNavigation { get; set; } = null!;
    }
}
