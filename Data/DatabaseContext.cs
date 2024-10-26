using Microsoft.EntityFrameworkCore;
using Models;
using Plan = back_sv_users.Models.Entities.Plan;
using Subscription = back_sv_users.Models.Entities.Subscription;
using UserEntity = back_sv_users.Models.Entities.UserEntity;

namespace back_SV_users.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Entrepreneurship> Entrepreneurships { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Subscription> Subscriptions {get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
      
      modelBuilder.Entity<UserEntity>()
    .Ignore(e => e.AdministrativeEmployee);

      modelBuilder.Entity<UserEntity>()
            .Ignore(e => e.AdministrativeEmployeeNavigation);

      
      modelBuilder.Entity<User>(entity =>
      {
            entity.ToTable("user_entity", schema: "public");

            entity.Property(u => u.Name)
                  .IsRequired() 
                  .HasMaxLength(100);

            entity.Property(u => u.Id_card)
                  .HasColumnName("id_card")
                  .IsRequired();

            entity.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(u => u.Password)
                  .IsRequired();

            entity.Property(u => u.RoleId)
                  .HasColumnName("role_id")
                  .IsRequired();
      });

      modelBuilder.Entity<Role>().ToTable("role", schema: "public");
      modelBuilder.Entity<Client>().ToTable("client", schema: "public");
      modelBuilder.Entity<Entrepreneurship>().ToTable("entrepreneurship", schema: "public");
      modelBuilder.Entity<Plan>().ToTable("plan", "public"); // Nombre y esquema de la tabla correctos
      modelBuilder.Entity<Subscription>().ToTable("subscription", "public"); // Configura el nombre y el esquema en minúsculas
      }


    }
}