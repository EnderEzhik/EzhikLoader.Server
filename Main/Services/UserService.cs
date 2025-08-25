using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EzhikLoader.Server.Data;
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

        public async Task<Models.DTOs.User.Response.UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"user with ID {userId} not found");
            }

            return _mapper.Map<Models.DTOs.User.Response.UserDTO>(user);
        }
    }
}
