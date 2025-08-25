using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Server.Interfaces;
using EzhikLoader.Server.Services;
using EzhikLoader.Server.Models.DTOs;

namespace EzhikLoader.Server.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsWebhookController : ControllerBase
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly IPaymentService _paymentService;

        public PaymentsWebhookController(SubscriptionService subscriptionService, IPaymentService paymentService)
        {
            _subscriptionService = subscriptionService;
            _paymentService = paymentService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandlePaymentWebhookAsync(WebhookRequest webhookRequest)
        {
            var fakePaymentService = (FakePaymentService)_paymentService;
            await fakePaymentService.UpdatePaymentStatus(webhookRequest.PaymentId, webhookRequest.Status);

            if (webhookRequest.Status == "success")
            {
                await _subscriptionService.ActivateAsync(webhookRequest.PaymentId);
            }

            return Ok();
        }
    }
}
