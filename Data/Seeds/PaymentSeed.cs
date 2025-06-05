using EzhikLoader.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace EzhikLoader.Server.Data.Seeds
{
    public static class PaymentSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().HasData(
                new Payment() { Id = 1, PaymentId = Guid.NewGuid().ToString(), UserId = 1, AppId = 1, IsManual = true, Amount = 0, Status = "success", CreatedAt = DateTime.UtcNow }
            );
        }
    }
}
