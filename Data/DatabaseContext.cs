using Microsoft.EntityFrameworkCore;
using Models;

namespace back_SV_users.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Entrepreneurship> Entrepreneurships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user_entity", schema: "public");
                entity.Property(u => u.Name)
                      .IsRequired() 
                      .HasMaxLength(100);

                entity.Property(u => u.Id_card)
                      .HasColumnName("id_card") // Mapeo de la columna
                      .IsRequired(); // Campo requerido

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
        }

    }
}