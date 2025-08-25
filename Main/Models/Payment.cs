namespace EzhikLoader.Server.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentId { get; set; }
        public bool IsManual { get; set; }
        public string Status { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int AppId { get; set; }
        public App App { get; set; }
    }
}
