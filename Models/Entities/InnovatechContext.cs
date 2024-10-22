using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace back_sv_users.Models.Entities;

public partial class InnovatechContext : DbContext
{
    public InnovatechContext()
    {
    }

    public InnovatechContext(DbContextOptions<InnovatechContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdministrativeEmployee> AdministrativeEmployees { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<CouponEntrepreneurship> CouponEntrepreneurships { get; set; }

    public virtual DbSet<CouponFunctionality> CouponFunctionalities { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseEntrepreneurship> CourseEntrepreneurships { get; set; }

    public virtual DbSet<Entrepreneurship> Entrepreneurships { get; set; }

    public virtual DbSet<EntrepreneurshipEventRegistry> EntrepreneurshipEventRegistries { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Functionality> Functionalities { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<PlanFunctionality> PlanFunctionalities { get; set; }

    public virtual DbSet<Publication> Publications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserEntity> UserEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=10.43.101.155;Database=innovatech;Username=postgres;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdministrativeEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("administrative_employee_pkey");

            entity.ToTable("administrative_employee");

            entity.HasIndex(e => e.UserId, "administrative_employee_user_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.AdministrativeEmployee)
                .HasForeignKey<AdministrativeEmployee>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fko8v1usqwn3nxmiw2brinilg5p");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCard)
                .HasColumnType("character varying")
                .HasColumnName("id_card");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("coupon_pkey");

            entity.ToTable("coupon");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("expiration_date");
            entity.Property(e => e.ExpirationPeriod).HasColumnName("expiration_period");
            entity.Property(e => e.IdEntrepreneurship).HasColumnName("id_entrepreneurship");
            entity.Property(e => e.IdPlan).HasColumnName("id_plan");

            entity.HasOne(d => d.IdEntrepreneurshipNavigation).WithMany(p => p.Coupons)
                .HasForeignKey(d => d.IdEntrepreneurship)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkqlpk6n5820kt361gn876fjftf");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.Coupons)
                .HasForeignKey(d => d.IdPlan)
                .HasConstraintName("fk8032kfswptfuu1145366ftqlk");
        });

        modelBuilder.Entity<CouponEntrepreneurship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("coupon_entrepreneurship_pkey");

            entity.ToTable("coupon_entrepreneurship");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.IdCoupon).HasColumnName("id_coupon");
            entity.Property(e => e.IdEntrepreneurship).HasColumnName("id_entrepreneurship");

            entity.HasOne(d => d.IdCouponNavigation).WithMany(p => p.CouponEntrepreneurships)
                .HasForeignKey(d => d.IdCoupon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk7w8id8svjk4uvayc2bqyjdf1d");

            entity.HasOne(d => d.IdEntrepreneurshipNavigation).WithMany(p => p.CouponEntrepreneurships)
                .HasForeignKey(d => d.IdEntrepreneurship)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkhh5wjg1bljw03tv2vn5srce9v");
        });

        modelBuilder.Entity<CouponFunctionality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("coupon_functionality_pkey");

            entity.ToTable("coupon_functionality");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCoupon).HasColumnName("id_coupon");
            entity.Property(e => e.IdFunctionality).HasColumnName("id_functionality");

            entity.HasOne(d => d.IdCouponNavigation).WithMany(p => p.CouponFunctionalities)
                .HasForeignKey(d => d.IdCoupon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkqe2jbyf25dpt31kw9my89jtsy");

            entity.HasOne(d => d.IdFunctionalityNavigation).WithMany(p => p.CouponFunctionalities)
                .HasForeignKey(d => d.IdFunctionality)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk6a6saycrr9mxg4yaik43e5aoa");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_pkey");

            entity.ToTable("course");

            entity.HasIndex(e => e.Title, "course_title_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Link)
                .HasMaxLength(255)
                .HasColumnName("link");
            entity.Property(e => e.Modality)
                .HasMaxLength(255)
                .HasColumnName("modality");
            entity.Property(e => e.Places).HasColumnName("places");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<CourseEntrepreneurship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_entrepreneurship_pkey");

            entity.ToTable("course_entrepreneurship");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.EntrepreneurshipId).HasColumnName("entrepreneurship_id");
            entity.Property(e => e.Puntaje).HasColumnName("puntaje");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseEntrepreneurships)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("fkc9944b6tjet15vigib9cbq4rc");

            entity.HasOne(d => d.Entrepreneurship).WithMany(p => p.CourseEntrepreneurships)
                .HasForeignKey(d => d.EntrepreneurshipId)
                .HasConstraintName("fk73b6ip3vqenl2xmk21s5fxutd");
        });

        modelBuilder.Entity<Entrepreneurship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("entrepreneurship_pkey");

            entity.ToTable("entrepreneurship");

            entity.HasIndex(e => e.Name, "entrepreneurship_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Lastnames)
                .HasMaxLength(255)
                .HasColumnName("lastnames");
            entity.Property(e => e.Logo)
                .HasMaxLength(255)
                .HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Names)
                .HasMaxLength(255)
                .HasColumnName("names");
        });

        modelBuilder.Entity<EntrepreneurshipEventRegistry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("entrepreneurship_event_registry_pkey");

            entity.ToTable("entrepreneurship_event_registry");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AmountPaid).HasColumnName("amount_paid");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date");
            entity.Property(e => e.IdEntrepreneurship).HasColumnName("id_entrepreneurship");
            entity.Property(e => e.IdEvent).HasColumnName("id_event");

            entity.HasOne(d => d.IdEntrepreneurshipNavigation).WithMany(p => p.EntrepreneurshipEventRegistries)
                .HasForeignKey(d => d.IdEntrepreneurship)
                .HasConstraintName("fk1wscbv7c7ljql1yn7gryr1jxj");

            entity.HasOne(d => d.IdEventNavigation).WithMany(p => p.EntrepreneurshipEventRegistries)
                .HasForeignKey(d => d.IdEvent)
                .HasConstraintName("fkrljd4l23e5d5l8h9bt4cj3hl9");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_pkey");

            entity.ToTable("event");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CostoLocal).HasColumnName("costo_local");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Date2)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date2");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Earnings).HasColumnName("earnings");
            entity.Property(e => e.Modality)
                .HasMaxLength(255)
                .HasColumnName("modality");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Place).HasColumnName("place");
            entity.Property(e => e.Quota).HasColumnName("quota");
            entity.Property(e => e.TotalCost).HasColumnName("total_cost");
        });

        modelBuilder.Entity<Functionality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("functionality_pkey");

            entity.ToTable("functionality");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plan_pkey");

            entity.ToTable("plan");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<PlanFunctionality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plan_functionality_pkey");

            entity.ToTable("plan_functionality");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FunctionalityId).HasColumnName("functionality_id");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");

            entity.HasOne(d => d.Functionality).WithMany(p => p.PlanFunctionalities)
                .HasForeignKey(d => d.FunctionalityId)
                .HasConstraintName("fk3kckbmw1sgvafmqyn2mixhlem");

            entity.HasOne(d => d.Plan).WithMany(p => p.PlanFunctionalities)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("fkf41bspsgwy5v5n2mym42ia1tk");
        });

        modelBuilder.Entity<Publication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("publication_pkey");

            entity.ToTable("publication");

            entity.HasIndex(e => e.Title, "publication_title_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdministrativeEmployeeId).HasColumnName("administrative_employee_id");
            entity.Property(e => e.Multimedia)
                .HasMaxLength(255)
                .HasColumnName("multimedia");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.AdministrativeEmployee).WithMany(p => p.Publications)
                .HasForeignKey(d => d.AdministrativeEmployeeId)
                .HasConstraintName("fkofgtg1y0cv4nsqy9igqkjixnt");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "role_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("subscription_pkey");

            entity.ToTable("subscription");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.EntrepreneurshipId).HasColumnName("entrepreneurship_id");
            entity.Property(e => e.ExpirationDate).HasColumnName("expiration_date");
            entity.Property(e => e.IdPlan).HasColumnName("id_plan");
            entity.Property(e => e.InitialDate).HasColumnName("initial_date");

            entity.HasOne(d => d.Entrepreneurship).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.EntrepreneurshipId)
                .HasConstraintName("fkjckkgi3ped0lv6yevi4f6mjka");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.IdPlan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkqmcvpkvxw4md0eya9u1xv3d9c");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_entity_pkey");

            entity.ToTable("user_entity");

            entity.HasIndex(e => e.AdministrativeEmployeeId, "user_entity_administrative_employee_id_key").IsUnique();

            entity.HasIndex(e => e.Email, "user_entity_email_key").IsUnique();

            entity.HasIndex(e => e.EntrepreneurshipId, "user_entity_entrepreneurship_id_key").IsUnique();

            entity.HasIndex(e => e.IdCard, "user_entity_id_card_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdministrativeEmployeeId).HasColumnName("administrative_employee_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EntrepreneurshipId).HasColumnName("entrepreneurship_id");
            entity.Property(e => e.IdCard).HasColumnName("id_card");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.AdministrativeEmployeeNavigation).WithOne(p => p.UserEntity)
                .HasForeignKey<UserEntity>(d => d.AdministrativeEmployeeId)
                .HasConstraintName("fk7f0g0o0keoihj7qmpxp2blua3");

            entity.HasOne(d => d.Entrepreneurship).WithOne(p => p.UserEntity)
                .HasForeignKey<UserEntity>(d => d.EntrepreneurshipId)
                .HasConstraintName("fkb8r80d91aont5jsq1mp7x3fh9");

            entity.HasOne(d => d.Role).WithMany(p => p.UserEntities)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkpostrnt7qdgc4m56i71qlkl61");
        });
        modelBuilder.HasSequence("course_entrepreneurship_seq").IncrementsBy(50);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
