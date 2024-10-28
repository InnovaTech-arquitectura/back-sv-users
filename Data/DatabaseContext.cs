using back_sv_users.Models.Entities;
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

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Entrepreneurship> Entrepreneurships { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Subscription> Subscriptions {get; set;}
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponEntrepreneurship> CouponEntrepreneurships { get; set; }  // Agregar la tabla CouponEntrepreneurship


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserEntity>()
    .Ignore(e => e.AdministrativeEmployee);

      modelBuilder.Entity<UserEntity>()
            .Ignore(e => e.AdministrativeEmployeeNavigation);

            modelBuilder.Entity<Models.User>(entity =>
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

                  // Configuración de otras entidades
                  modelBuilder.Entity<Models.Role>().ToTable("role", schema: "public");
                  modelBuilder.Entity<Client>().ToTable("client", schema: "public");
                  modelBuilder.Entity<Entrepreneurship>().ToTable("entrepreneurship", schema: "public");
                  modelBuilder.Entity<Plan>().ToTable("plan", "public"); // Nombre y esquema de la tabla correctos
                  modelBuilder.Entity<Subscription>().ToTable("subscription", "public"); // Configura el nombre y el esquema en minúsculas
                  modelBuilder.Entity<Coupon>().ToTable("coupon", schema: "public");

                  // Configuración de la entidad CouponEntrepreneurship
                  modelBuilder.Entity<CouponEntrepreneurship>(entity =>
      {
            entity.ToTable("coupon_entrepreneurship", schema: "public");
            entity.HasKey(ce => ce.id);

            entity.Property(e => e.id)
            .HasColumnName("id");

            entity.Property(e => e.IdCoupon)
            .HasColumnName("id_coupon");

            entity.Property(e => e.IdEntrepreneurship)
            .HasColumnName("id_entrepreneurship");

            entity.Property(e => e.Active)
            .HasColumnName("active");

            // Configuración de las relaciones
            entity.HasOne(ce => ce.Coupon)
            .WithMany(c => c.CouponEntrepreneurships)
            .HasForeignKey(ce => ce.IdCoupon)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ce => ce.Entrepreneurship)
            .WithMany(e => e.CouponEntrepreneurships)
            .HasForeignKey(ce => ce.IdEntrepreneurship)
            .OnDelete(DeleteBehavior.Cascade);
      });

            }
      }
}
