using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models.DTOs.Admin.Request;
using EzhikLoader.Server.Models.DTOs.Admin.Response;
using EzhikLoader.Server.Exceptions;

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

        public async Task<Models.DTOs.User.Response.UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"user with ID {userId} not found");
            }

            return _mapper.Map<Models.DTOs.User.Response.UserDTO>(user);
        }

        public async Task<UserDTO> GetDetailedUserByIdAsync(int userId)
        {
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"user with ID {userId} not found");
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateUserAsync(UpdateUserDTO userDTO)
        {
            var user = await _dbContext.Users.Include(e => e.Role).FirstOrDefaultAsync(u => u.Id == userDTO.Id);
            if (user == null)
            {
                throw new NotFoundException($"user with ID {userDTO.Id} not found");
            }

            _mapper.Map(userDTO, user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserDTO> DeleteUserByIdAsync(int userId)
        {
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"user with ID {userId} not found");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }
    }
}
