using System.ComponentModel.DataAnnotations;

namespace EzhikLoader.Server.Models.DTOs.Admin.Request
{
    public class UpdateAppDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(5, ErrorMessage = "The minimum length of the name is 5")]
        [MaxLength(25, ErrorMessage = "The maximum length of the name is 25")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MinLength(20, ErrorMessage = "The minimum length of the description is 20")]
        [MaxLength(150, ErrorMessage = "The maximum length of the description is 150")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Version is required")]
        [Length(5, 5, ErrorMessage = "Incorrect version format")]
        public string Version { get; set; }

        [Required]
        public double? Price { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MinLength(5, ErrorMessage = "The minimum length of the filename is 5")]
        [MaxLength(25, ErrorMessage = "The maximum length of the filename is 25")]
        public string FileName { get; set; }

        [MinLength(5, ErrorMessage = "The minimum length of the iconname is 5")]
        [MaxLength(25, ErrorMessage = "The maximum length of the iconname is 25")]
        public string? IconName { get; set; }
    }
}
