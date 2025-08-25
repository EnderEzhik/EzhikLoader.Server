using EzhikLoader.Server.Models.DTOs.User.Request;
using EzhikLoader.Server.Models.DTOs.User.Response;

namespace EzhikLoader.Server.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> StartPaymentAsync(int userId, int appId);

        Task<bool> CheckPaymentStatusAsync(string paymentId);
    }
}
