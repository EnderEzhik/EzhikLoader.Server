using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Interfaces;
using EzhikLoader.Server.Models;
using EzhikLoader.Server.Models.DTOs.Request;
using EzhikLoader.Server.Models.DTOs.Response;

namespace EzhikLoader.Server.Services
{
    public class FakePaymentService : IPaymentService
    {
        private readonly MyDbContext _dbContext;

        public FakePaymentService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaymentResponse> StartPaymentAsync(PaymentRequest paymentRequest)
        {
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid().ToString(),
                UserId = paymentRequest.UserId,
                AppId = paymentRequest.AppId,
                Status = "pending",
                Amount = paymentRequest.Amount,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();

            return new PaymentResponse
            {
                PaymentId = payment.PaymentId,
                PaymentUrl = $"https://youtube.com"
            };
        }

        public async Task<bool> CheckPaymentStatusAsync(string paymentId)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null || payment.Status != "success")
            {
                return false;
            }

            return true;
        }

        public async Task UpdatePaymentStatus(string paymentId, string status)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            if (payment != null)
            {
                payment.Status = status;

                await _dbContext.SaveChangesAsync();
            }
        }

        //public async void SetPaymentSuccess(string paymentId, string status)
        //{
        //    var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        //    if (payment != null)
        //    {
        //        payment.Status = status;

        //        await _dbContext.SaveChangesAsync();

        //        if (payment.Status == "success")
        //        {
        //            await _subscriptionService.ActivateAsync(paymentId);
        //        }
        //    }
        //}
    }
}
