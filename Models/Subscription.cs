namespace EzhikLoader.Server.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? LastDownloadedAt { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int AppId { get; set; }
        public virtual App App { get; set; }

        public int PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
