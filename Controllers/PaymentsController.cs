using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Server.Interfaces;
using EzhikLoader.Server.Models.DTOs.Request;
using EzhikLoader.Server.Services;

namespace EzhikLoader.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly SubscriptionService _subscriptionService;

        public PaymentsController(IPaymentService paymentService, SubscriptionService subscriptionService)
        {
            _paymentService = paymentService;
            _subscriptionService = subscriptionService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start(PaymentRequest paymentRequest)
        {
            var response = await _paymentService.StartPaymentAsync(paymentRequest);
            return Ok(response);
        }

        [HttpGet("status/{paymentId}")]
        public async Task<IActionResult> CheckStatus(string paymentId)
        {
            var isPaid = await _paymentService.CheckPaymentStatusAsync(paymentId);
            return Ok(new { IsPaid = isPaid});
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandlePaymentWebhookAsync(WebhookRequest webhookRequest)
        {
            var fakePaymentService = (FakePaymentService) _paymentService;
            await fakePaymentService.UpdatePaymentStatus(webhookRequest.PaymentId, webhookRequest.Status);

            if(webhookRequest.Status == "success")
            {
                await _subscriptionService.ActivateAsync(webhookRequest.PaymentId);
            }

            return Ok();
        }
    }
}
