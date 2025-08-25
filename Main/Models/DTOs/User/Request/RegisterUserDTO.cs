namespace EzhikLoader.Server.Models.DTOs.User.Request
{
    public class RegisterUserDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
    }
}
