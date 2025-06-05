using EzhikLoader.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace EzhikLoader.Server.Data.Seeds
{
    public static class AppSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<App>().HasData(
                new App() { Id = 1, Name = "testApp1", Description = "Description for testApp1", Version = "0.0.1", Price = 199, IsActive = true, FileName = "testApp1.txt" },
                new App() { Id = 2, Name = "testApp2", Description = "Description for testApp2", Version = "0.0.2", Price = 299, IsActive = true, FileName = "testApp2.txt" },
                new App() { Id = 3, Name = "testApp3", Description = "Description for testApp3", Version = "0.0.1", Price = 399, IsActive = true, FileName = "testApp3.txt" }
            );
        }
    }
}
