namespace EzhikLoader.Server.Models.DTOs.User.Response
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string? Email { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
