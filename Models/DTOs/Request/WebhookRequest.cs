namespace EzhikLoader.Server.Models.DTOs.Request
{
    public class WebhookRequest
    {
        public string PaymentId { get; set; }
        public string Status {  get; set; }
        public decimal Amount { get; set; }
    }
}
