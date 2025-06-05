namespace EzhikLoader.Server.Models.DTOs.Admin.Request
{
    public class UpdateAppDataDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public double? Price { get; set; }
        public bool? IsActive { get; set; }
        public string? FileName { get; set; }
        public string? IconName { get; set; }
    }
}
