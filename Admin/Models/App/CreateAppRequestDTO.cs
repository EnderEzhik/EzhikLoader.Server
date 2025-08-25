namespace EzhikLoader.Admin.Models.App
{
    /// <summary>
    /// DTO для создания нового приложения
    /// </summary>
    public class CreateAppRequestDTO
    {
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
        /// Имя файла приложения
        /// </summary>
        public string FileName { get; set; } = null!;

        /// <summary>
        /// Имя иконки приложения (опционально)
        /// </summary>
        public string? IconName { get; set; }
    }
}
