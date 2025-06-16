using System.ComponentModel.DataAnnotations;

namespace EzhikLoader.Server.Models.DTOs.Admin.Request
{
    public class CreateUserDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }

        [Required]
        public bool? IsActive { get; set; }
    }
}
