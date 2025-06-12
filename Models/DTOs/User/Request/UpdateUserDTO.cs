namespace EzhikLoader.Server.Models.DTOs.User.Request
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
