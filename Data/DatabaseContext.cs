using back_sv_users.Models.Entities;
using back_sv_Users.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Models;

namespace back_SV_users.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Entrepreneurship> Entrepreneurships { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponEntrepreneurship> CouponEntrepreneurships { get; set; }  // Agregar la tabla CouponEntrepreneurship

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la entidad User
            modelBuilder.Entity<Models.User>(entity =>
            {
                entity.ToTable("user_entity", schema: "public");
                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(100);

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
            modelBuilder.Entity<Coupon>().ToTable("coupon", schema: "public");

            // Configuración de la entidad CouponEntrepreneurship
            modelBuilder.Entity<CouponEntrepreneurship>(entity =>
            {
                entity.ToTable("coupon_entrepreneurship", schema: "public");
                entity.HasKey(ce => ce.Id);

                // Relación con la tabla Coupon
                entity.HasOne(ce => ce.Coupon) // Cambiado a Coupon
                      .WithMany(c => c.CouponEntrepreneurships)
                      .HasForeignKey(ce => ce.IdCoupon)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con la tabla Entrepreneurship
                entity.HasOne(ce => ce.Entrepreneurship) // Cambiado a Entrepreneurship
                      .WithMany(e => e.CouponEntrepreneurships)
                      .HasForeignKey(ce => ce.IdEntrepreneurship)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
