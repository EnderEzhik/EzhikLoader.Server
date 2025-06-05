using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Server.Interfaces;
using EzhikLoader.Server.Models.DTOs.User.Request;

namespace EzhikLoader.Server.Controllers.User
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start(PaymentRequest paymentRequest)
        {
            try
            {
                var response = await _paymentService.StartPaymentAsync(paymentRequest);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("status/{paymentId}")]
        public async Task<IActionResult> CheckPaymentStatus(string paymentId)
        {
            var result = await _paymentService.CheckPaymentStatusAsync(paymentId);
            return Ok(new { IsPaid = result});
        }
    }
}
