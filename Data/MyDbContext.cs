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
