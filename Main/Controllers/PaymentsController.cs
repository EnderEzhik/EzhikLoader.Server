using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Server.Interfaces;
using EzhikLoader.Server.Models.DTOs.User.Request;

namespace EzhikLoader.Server.Controllers
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
        public async Task<IActionResult> Start(PaymentRequest request)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst("sub");
                int userId = int.Parse(userIdClaim.Value);
                var response = await _paymentService.StartPaymentAsync(userId, request.AppId);
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
