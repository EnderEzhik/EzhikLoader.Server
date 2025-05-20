using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Server.Models.DTOs.Response;
using EzhikLoader.Server.Models.DTOs.Request;

namespace EzhikLoader.Server.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> StartPaymentAsync(PaymentRequest paymentRequest);

        Task<bool> CheckPaymentStatusAsync(string paymentId);
    }
}
