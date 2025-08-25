namespace EzhikLoader.Admin.Entities
{
    public class App
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Version { get; set; } = null!;
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string FileName { get; set; } = null!;
        public string? IconName { get; set; }
    }
}
