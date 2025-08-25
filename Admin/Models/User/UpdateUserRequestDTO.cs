namespace EzhikLoader.Admin.Models.User
{
    /// <summary>
    /// DTO для обновления пользователя.
    /// Все поля nullable - если поле не передано (null), то оно не будет изменено.
    /// </summary>
    public class UpdateUserRequestDTO
    {
        /// <summary>
        /// Новый логин. Если null, то логин не изменяется.
        /// </summary>
        public string? Login { get; set; }

        /// <summary>
        /// Новый email. Если null, то email не изменяется.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Новое состояние активности. Если null, то состояние не изменяется.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Новая дата последнего входа. Если null, то дата не изменяется.
        /// </summary>
        public DateTime? LastLoginDate { get; set; }
    }
}
