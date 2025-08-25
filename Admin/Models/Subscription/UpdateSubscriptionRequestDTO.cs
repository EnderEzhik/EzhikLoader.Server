namespace EzhikLoader.Admin.Models.Subscription
{
    /// <summary>
    /// DTO для обновления подписки.
    /// Все поля nullable - если поле не передано (null), то оно не будет изменено.
    /// </summary>
    public class UpdateSubscriptionRequestDTO
    {
        /// <summary>
        /// Новая дата начала подписки. Если null, то дата не изменяется.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Новая дата окончания подписки. Если null, то дата не изменяется.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Новая дата последней загрузки. Если null, то дата не изменяется.
        /// </summary>
        public DateTime? LastDownloadedAt { get; set; }

        /// <summary>
        /// Новое состояние отмены. Если null, то состояние не изменяется.
        /// </summary>
        public bool? IsCanceled { get; set; }
    }
}
