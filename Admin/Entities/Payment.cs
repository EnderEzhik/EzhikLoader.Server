namespace EzhikLoader.Admin.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentId { get; set; } = null!;
        public bool IsManual { get; set; }
        public string Status { get; set; } = null!;
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int AppId { get; set; }
        public App App { get; set; } = null!;
    }
}
