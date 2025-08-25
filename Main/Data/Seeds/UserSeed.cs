using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Data.Seeds
{
    public static class UserSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Login = "test", Password = "test", RoleId = 1, IsActive = true },
                new User() { Id = 2, Login = "S1NX0FAZATR0N", Password = "122333444455555", RoleId = 2, IsActive = true }
            );
        }
    }
}
