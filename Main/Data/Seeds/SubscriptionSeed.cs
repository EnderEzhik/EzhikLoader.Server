using EzhikLoader.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace EzhikLoader.Server.Data.Seeds
{
    public static class SubscriptionSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>().HasData(
                new Subscription() { Id = 1, UserId = 1, AppId = 1, PaymentId = 1, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), IsCanceled = false },
                new Subscription() { Id = 2, UserId = 1, AppId = 2, PaymentId = 2, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), IsCanceled = false },
                new Subscription() { Id = 3, UserId = 1, AppId = 4, PaymentId = 3, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), IsCanceled = false }
            );
        }
    }
}
