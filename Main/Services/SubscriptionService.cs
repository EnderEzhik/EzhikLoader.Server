using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Services
{
    public class SubscriptionService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;

        public SubscriptionService(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task ActivateAsync(string paymentId)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
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
