using EzhikLoader.Server.Models.DTOs.User.Request;
using EzhikLoader.Server.Models.DTOs.User.Response;

namespace EzhikLoader.Server.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> StartPaymentAsync(PaymentRequest paymentRequest);

        Task<bool> CheckPaymentStatusAsync(string paymentId);
    }
}
