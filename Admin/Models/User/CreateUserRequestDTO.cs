using System.ComponentModel.DataAnnotations;

namespace EzhikLoader.Admin.Models.User
{
    public class CreateUserRequestDTO
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
    }
}
