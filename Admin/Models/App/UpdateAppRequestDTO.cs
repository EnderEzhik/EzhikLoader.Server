namespace EzhikLoader.Admin.Models.App
{
    /// <summary>
    /// DTO для обновления приложения.
    /// Все поля nullable - если поле не передано (null), то оно не будет изменено.
    /// </summary>
    public class UpdateAppRequestDTO
    {
        /// <summary>
        /// Новое название приложения. Если null, то название не изменяется.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Новое описание приложения. Если null, то описание не изменяется.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Новая версия приложения. Если null, то версия не изменяется.
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// Новая цена приложения. Если null, то цена не изменяется.
        /// </summary>
        public double? Price { get; set; }

        /// <summary>
        /// Новое состояние активности. Если null, то состояние не изменяется.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Новое имя файла приложения. Если null, то имя файла не изменяется.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Новое имя иконки приложения. Если null, то имя иконки не изменяется.
        /// </summary>
        public string? IconName { get; set; }
    }
}
