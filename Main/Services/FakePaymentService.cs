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
        //TODO: добавить проверку наличия подписки у пользователя на приложение
        public async Task<PaymentResponse> StartPaymentAsync(int userId, int appId)
        {
            if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
            {
                throw new ArgumentException($"user with ID \"{userId}\" not found");
            }

            var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Id == appId);
            if(app == null)
            {
                throw new ArgumentException($"app with ID \"{appId}\" not found");
            }

            var payment = new Models.Payment
            {
                PaymentId = Guid.NewGuid().ToString(),
                UserId = userId,
                AppId = appId,
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
