namespace EzhikLoader.Admin.Models.App
{
    /// <summary>
    /// DTO для ответа с данными приложения
    /// </summary>
    public class AppResponseDTO
    {
        /// <summary>
        /// ID приложения
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название приложения
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Описание приложения
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Версия приложения
        /// </summary>
        public string Version { get; set; } = null!;

        /// <summary>
        /// Цена приложения
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Активно ли приложение
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Имя файла приложения
        /// </summary>
        public string FileName { get; set; } = null!;

        /// <summary>
        /// Имя иконки приложения
        /// </summary>
        public string? IconName { get; set; }
    }
}
