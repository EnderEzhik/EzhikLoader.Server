namespace EzhikLoader.Admin.Models.Subscription
{
    /// <summary>
    /// DTO для создания новой подписки
    /// </summary>
    public class CreateSubscriptionRequestDTO
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// ID приложения
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// ID платежа
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        /// Дата начала подписки
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания подписки
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
