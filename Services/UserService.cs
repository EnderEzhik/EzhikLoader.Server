using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models.DTOs.Admin.Request;
using EzhikLoader.Server.Models.DTOs.Admin.Response;

namespace EzhikLoader.Server.Services
{
    public class UserService
    {
        private readonly MyDbContext _dbContext;
        private readonly AuthService _authService;
        private readonly IMapper _mapper;

        public UserService(MyDbContext dbContext, AuthService authService, IMapper mapper)
        {
            _dbContext = dbContext;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO newUserDTO)
        {
            var newUser = await _authService.RegisterAsync(newUserDTO.Login, newUserDTO.Password, newUserDTO.Email);
            return _mapper.Map<UserDTO>(newUser);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _dbContext.Users.Include(u => u.Role).ToListAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<Models.DTOs.User.Response.UserDTO> GetUserByIDAsync(int userId)
        {
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentException($"user with ID {userId} not found");
            }

            return _mapper.Map<Models.DTOs.User.Response.UserDTO>(user);
        }

        public async Task UpdateUserAsync(UpdateUserDTO updateUser)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == updateUser.Id);
            if (user == null)
            {
                throw new ArgumentException($"user with ID {updateUser.Id} not found");
            }

            if (updateUser.Login != null)
            {
                user.Login = updateUser.Login;
            }
            if (updateUser.Password != null)
            {
                user.Password = updateUser.Password;
            }
            if (updateUser.Email != null)
            {
                user.Email = updateUser.Email;
            }
            if (updateUser.RoleId.HasValue)
            {
                user.RoleId = updateUser.RoleId.Value;
            }
            if (updateUser.IsActive.HasValue)
            {
                user.IsActive = updateUser.IsActive.Value;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserDTO> DeleteUserByIDAsync(int userId)
        {
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException($"user with ID {userId} not found");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }
    }
}
