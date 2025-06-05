using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Models;
using EzhikLoader.Server.Data.Seeds;

namespace EzhikLoader.Server.Data
{
    public class MyDbContext: DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<App> Apps { get; set; } = null!;
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        public DbSet<Role> Roles { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> contextOptions) : base(contextOptions) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=ezhik_loader;", 
                new MySqlServerVersion(new Version(8, 4, 5)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);

            RoleSeed.Seed(modelBuilder);
            UserSeed.Seed(modelBuilder);
            AppSeed.Seed(modelBuilder);
            PaymentSeed.Seed(modelBuilder);
            SubscriptionSeed.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
