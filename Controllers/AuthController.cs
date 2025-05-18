using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Services;
using EzhikLoader.Server.Models;
using EzhikLoader.Server.Models.DTOs.Request;

namespace EzhikLoader.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly MyDbContext _dbContext;
        public AuthController(IJwtService jwtService, MyDbContext dbContext)
        {
            _jwtService = jwtService;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            if(string.IsNullOrEmpty(loginUser.login) || string.IsNullOrEmpty(loginUser.password))
            {
                return BadRequest();
            }

            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(user => user.Login == loginUser.login && user.Password == loginUser.password);
            
            if (user == null)
            {
                return Unauthorized();
            }

            string token = _jwtService.GenerateToken(user);
            return Ok(new { token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO userDTO)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == userDTO.Login);
            if (existUser !=  null)
            {
                return BadRequest($"User with login \"{userDTO.Login}\" already exist");
            }

            User newUser = new User();
            newUser.Login = userDTO.Login;
            newUser.Password = userDTO.Password;
            newUser.Role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "user");

            if (userDTO.Email != null)
            {
                var existEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
                if (existEmail != null)
                {
                    return BadRequest($"Email \"{userDTO.Email}\" is already in use");
                }

                newUser.Email = userDTO.Email;
            }
            
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            string token = _jwtService.GenerateToken(newUser);
            return Ok(new { token = token });
        }
    }
}
