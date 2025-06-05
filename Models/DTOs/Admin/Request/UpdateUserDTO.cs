namespace EzhikLoader.Server.Models.DTOs.Admin.Request
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}
