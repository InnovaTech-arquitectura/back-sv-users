using Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace back_SV_users
{
    public class Entrepreneurship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")] 
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")] 
        [Column("user_entity_id")]
        public int Id_user { get; set; }

        /*
        [Required]
        [ForeignKey("Plan")]
        [Column("id_plan")] 
        public int Id_plan { get; set; }
        */

        [Required]
        [Column("name")]
        public required string Name { get; set; }
        
        [Required]
        [Column("names")]
        public required string Names {get;set;}
        
        [Required]
        [Column("lastnames")]
        public required string LastNames {get;set;}

        [Column("logo")]
        public string Logo { get; set; }

        [Column("description")]
        public required string Description { get; set; }

        public User User { get; set; }
    }
}
