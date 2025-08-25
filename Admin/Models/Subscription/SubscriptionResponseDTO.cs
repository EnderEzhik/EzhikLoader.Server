namespace EzhikLoader.Admin.Models.Subscription
{
    /// <summary>
    /// DTO для ответа с данными подписки
    /// </summary>
    public class SubscriptionResponseDTO
    {
        /// <summary>
        /// ID подписки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата начала подписки
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания подписки
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Дата последней загрузки
        /// </summary>
        public DateTime? LastDownloadedAt { get; set; }

        /// <summary>
        /// Отменена ли подписка
        /// </summary>
        public bool IsCanceled { get; set; }

        /// <summary>
        /// ID пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserLogin { get; set; } = null!;

        /// <summary>
        /// ID приложения
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// Название приложения
        /// </summary>
        public string AppName { get; set; } = null!;

        /// <summary>
        /// ID платежа
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        /// Активна ли подписка (не отменена и не истекла)
        /// </summary>
        public bool IsActive { get; set; }
    }
}
