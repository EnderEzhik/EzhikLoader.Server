namespace EzhikLoader.Server.Models.DTOs.Request
{
    public class PaymentRequest
    {
        public int UserId { get; set; }
        public int AppId { get; set; }
        public decimal Amount { get; set; }
    }
}
