using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models.DTOs;
using EzhikLoader.Server.Services;

namespace EzhikLoader.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> Register()
        {
            return Ok();
        }
    }
}
