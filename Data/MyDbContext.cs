using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Data
{
    public class MyDbContext: DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<App> Apps { get; set; } = null!;
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        public DbSet<Role> Roles { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> contextOptions) : base(contextOptions)
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=ezhik_loader;", 
                new MySqlServerVersion(new Version(8, 4, 5)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Login).HasMaxLength(20).IsRequired();
                entity.HasIndex(entity => entity.Login).IsUnique();

                entity.Property(e => e.Password).HasMaxLength(32).IsRequired();

                entity.Property(e => e.Email).HasMaxLength(255);
                entity.HasIndex(entity => entity.Email).IsUnique();

                entity.Property(e => e.RoleId).IsRequired();

                entity.Property(e => e.IsActive).IsRequired();

                entity.Property(e => e.RegistrationDate).HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP()");

                entity.Property(e => e.LastLoginDate).HasColumnType("TIMESTAMP");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasMaxLength(32).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<App>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasMaxLength(32).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.Description).HasColumnType("TEXT").IsRequired();

                entity.Property(e => e.Price).HasColumnType("DECIMAL(6, 2)").IsRequired();

                entity.Property(e => e.IsActive).IsRequired();

                entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP()");

                entity.Property(e => e.LastUpdatedAt).HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP()");

                entity.Property(e => e.FileName).HasMaxLength(32).IsRequired();
                entity.HasIndex(e => e.FileName).IsUnique();

                entity.Property(e => e.IconName).HasMaxLength(32).IsRequired();
                entity.HasIndex(e => e.IconName).IsUnique();
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.UserId).IsRequired();

                entity.Property(e => e.AppId).IsRequired();
                
                entity.Property(e => e.StartDate).HasColumnType("TIMESTAMP").IsRequired();

                entity.Property(e => e.EndDate).HasColumnType("TIMESTAMP").IsRequired();
                
                entity.Property(e => e.LastDownloadedAt).HasColumnType("TIMESTAMP");
            });

            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = 1, Name = "admin" },
                new Role() { Id = 2, Name = "user" }
            );

            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Login = "test", Password = "test", RoleId = 1, IsActive = true}
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
