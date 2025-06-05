using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models;
using EzhikLoader.Server.Models.DTOs.Admin.Request;

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

        public async Task<Models.DTOs.Admin.Response.SubscriptionDTO> GiveSubscription(int userId, int appId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException($"user with ID {userId} not found");
            }

            var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Id == appId);
            if (app == null)
            {
                throw new ArgumentException($"app with ID {appId} not found");
            }

            Payment newPayment = new Payment();
            newPayment.PaymentId = Guid.NewGuid().ToString();
            newPayment.UserId = userId;
            newPayment.AppId = appId;
            newPayment.IsManual = true;
            newPayment.Status = "success";
            newPayment.Amount = 0;

            _dbContext.Payments.Add(newPayment);
            await _dbContext.SaveChangesAsync();

            Subscription newSubscription = new Subscription();
            newSubscription.UserId = userId;
            newSubscription.AppId = appId;
            newSubscription.PaymentId = newPayment.Id;
            newSubscription.StartDate = DateTime.UtcNow;
            newSubscription.EndDate = DateTime.UtcNow.AddMonths(1);

            _dbContext.Subscriptions.Add(newSubscription);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Models.DTOs.Admin.Response.SubscriptionDTO>(newSubscription);
        }

        public async Task<Models.DTOs.Admin.Response.SubscriptionDTO> RevokeSubscription(int subscriptionId)
        {
            var subscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == subscriptionId);
            if (subscription == null)
            {
                throw new ArgumentException($"subscription with ID {subscriptionId} not found");
            }

            subscription.IsCanceled = true;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Models.DTOs.Admin.Response.SubscriptionDTO>(subscription);
        }

        public async Task ExtendSubscription(ExtendSubscriptionDTO extendSubscription)
        {
            var subscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == extendSubscription.SubscriptionId);
            if (subscription == null)
            {
                throw new ArgumentException($"subscription with ID {extendSubscription.SubscriptionId} not found");
            }

            subscription.EndDate = extendSubscription.NewEndDate;
            await _dbContext.SaveChangesAsync();
        }
    }
}
