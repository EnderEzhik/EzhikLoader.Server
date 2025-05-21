using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Services
{
    public class AuthService
    {
        private readonly MyDbContext _dbContext;

        public AuthService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> Login(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("login and password are required");
            }

            var user = await _dbContext.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(user => user.Login == login && user.Password == password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("login or password incorrect");
            }

            return user;
        }

        public async Task<User> Register(string login, string password, string? email = null)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == login);
            if (existUser != null)
            {
                throw new ArgumentException($"User with login \"{login}\" already exist");
            }

            User newUser = new User();
            newUser.Login = login;
            newUser.Password = password;
            newUser.Role = await _dbContext.Roles.FirstAsync(r => r.Name == "user");

            if (email != null)
            {
                var existEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existEmail != null)
                {
                    throw new ArgumentException($"Email \"{email}\" is already in use");
                }

                newUser.Email = email;
            }

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return newUser;
        }
    }
}
