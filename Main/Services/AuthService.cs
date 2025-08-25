using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models;
using EzhikLoader.Server.Exceptions;

namespace EzhikLoader.Server.Services
{
    public class AuthService
    {
        private readonly MyDbContext _dbContext;

        public AuthService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> LoginAsync(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                throw new BadRequestException("login and password are required");
            }

            var user = await _dbContext.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(user => user.Login == login && user.Password == password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("login or password incorrect");
            }

            user.LastLoginDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> RegisterAsync(string login, string password, string? email = null)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == login);
            if (existUser != null)
            {
                throw new BadRequestException($"user with login \"{login}\" already exist");
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
                    throw new BadRequestException($"email \"{email}\" is already in use");
                }

                newUser.Email = email;
            }

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return newUser;
        }

        public async Task ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"user with ID {userId} not found");
            }

            if (user.Password == newPassword)
            {
                throw new BadRequestException("the new password must be different from the old one");
            }

            user.Password = newPassword;
            await _dbContext.SaveChangesAsync();
        }

        internal async Task LinkEmailAsync(int userId, string email)
        {
            var emailExist = await _dbContext.Users.AnyAsync(u => u.Email == email);
            if(emailExist)
            {
                throw new BadRequestException($"email \"{email}\" is already in use");
            }

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"user with ID {userId} not found");
            }

            if (user.Email == email)
            {
                throw new BadRequestException("the new email must be different from the old one");
            }

            user.Email = email;
            await _dbContext.SaveChangesAsync();
        }
    }
}
