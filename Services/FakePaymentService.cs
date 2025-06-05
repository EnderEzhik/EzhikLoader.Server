using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Interfaces;
using EzhikLoader.Server.Models.DTOs.User.Response;
using EzhikLoader.Server.Models.DTOs.User.Request;

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
            if (!await _dbContext.Users.AnyAsync(u => u.Id == paymentRequest.UserId))
            {
                throw new ArgumentException($"user with ID \"{paymentRequest.UserId}\" not found");
            }

            var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Id == paymentRequest.AppId);
            if(app == null)
            {
                throw new ArgumentException($"app with ID \"{paymentRequest.AppId}\" not found");
            }

            var payment = new Models.Payment
            {
                PaymentId = Guid.NewGuid().ToString(),
                UserId = paymentRequest.UserId,
                AppId = paymentRequest.AppId,
                Status = "pending",
                Amount = app.Price,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();

            string paymentUrl = $"https://t.me/TestMyAllTelegram_bot?start={payment.PaymentId}";

            return new PaymentResponse
            {
                PaymentId = payment.PaymentId,
                PaymentUrl = paymentUrl
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
    }
}
