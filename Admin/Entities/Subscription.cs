namespace EzhikLoader.Admin.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? LastDownloadedAt { get; set; }
        public bool IsCanceled { get; set; }

        public int UserId { get; set; }

        public int AppId { get; set; }

        public int PaymentId { get; set; }
    }
}
