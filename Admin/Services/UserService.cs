using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EzhikLoader.Admin.Database;
using EzhikLoader.Admin.Entities;
using EzhikLoader.Admin.Models.User;
using EzhikLoader.Admin.Exceptions;

namespace EzhikLoader.Admin.Services
{
    public class UserService
    {
        public readonly MyDbContext db;
        public readonly IMapper automapper;
        private readonly string defaultRoleName;

        public UserService(MyDbContext _db, IMapper _automapper, IConfiguration configuration)
        {
            db = _db;
            automapper = _automapper;
            defaultRoleName = configuration["DefaultRoleName"] ?? throw new ArgumentNullException("DefaultRoleName can not be null");
        }

        public async Task<UserResponseDTO> CreateUserAsync(CreateUserRequestDTO userDTO)
        {
            bool loginExist = await db.Users.AnyAsync(u => u.Login == userDTO.Login);
            if (loginExist)
            {
                throw new BadRequestException($"user with login \"{userDTO.Login}\" is already exist");
            }

            if (!string.IsNullOrEmpty(userDTO.Email))
            {
                bool emailExist = await db.Users.AnyAsync(u => u.Email == userDTO.Email);
                if (emailExist)
                {
                    throw new BadRequestException($"email \"{userDTO.Email}\" is already in use");
                }
            }

            Role? defaultRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == defaultRoleName);
            if (defaultRole == null)
            {
                defaultRole = new Role()
                {
                    Name = defaultRoleName
                };
                db.Roles.Add(defaultRole);
                await db.SaveChangesAsync();
            }

            User user = automapper.Map<User>(userDTO);
            user.RoleId = defaultRole.Id;
            user.RegistrationDate = DateTime.UtcNow;
            user.IsActive = true;

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return automapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
        {
            User? user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return automapper.Map<UserResponseDTO>(user);
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await db.Users.ToListAsync();
            return users.Select(u => automapper.Map<UserResponseDTO>(u));
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            User? user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<UserResponseDTO?> UpdateUserAsync(int id, UpdateUserRequestDTO userUpdate)
        {
            User? user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            if (userUpdate.Login != null)
            {
                user.Login = userUpdate.Login;
            }

            if (userUpdate.Email != null)
            {
                user.Email = userUpdate.Email;
            }

            if (userUpdate.IsActive.HasValue)
            {
                user.IsActive = userUpdate.IsActive.Value;
            }

            if (userUpdate.LastLoginDate.HasValue)
            {
                user.LastLoginDate = userUpdate.LastLoginDate.Value;
            }

            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return automapper.Map<UserResponseDTO>(user);
        }
    }
}
