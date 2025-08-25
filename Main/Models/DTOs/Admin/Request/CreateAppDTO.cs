using System.ComponentModel.DataAnnotations;

namespace EzhikLoader.Server.Models.DTOs.Admin.Request
{
    public class CreateAppDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }

        [Required]
        public double? Price { get; set; }

        [Required]
        public bool? IsActive { get; set; }
        public IFormFile File { get; set; }
        public IFormFile? Icon { get; set; }
    }
}
