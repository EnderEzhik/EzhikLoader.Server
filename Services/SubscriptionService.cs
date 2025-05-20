using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Interfaces;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Services
{
    public class SubscriptionService
    {
        private readonly MyDbContext _dbContext;

        public SubscriptionService(MyDbContext dbContext, IPaymentService paymentService)
        {
            _dbContext = dbContext;
        }

        public async Task ActivateAsync(string paymentId)
        {
            var payment = await _dbContext.Payments
                .Include(p => p.User)
                .Include(p => p.App)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null || payment.Status != "success")
            {
                return;
            }

            var subscription = new Subscription
            {
                UserId = payment.UserId,
                AppId = payment.AppId,
                PaymentId = payment.Id,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            };

            _dbContext.Subscriptions.Add(subscription);
            await _dbContext.SaveChangesAsync();
        }
    }
}
