using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Server.Services;
using EzhikLoader.Server.Models.DTOs.User.Request;
using EzhikLoader.Server.Exceptions;

namespace EzhikLoader.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly AuthService _authService;

        public AuthController(IJwtService jwtService, AuthService authService)
        {
            _jwtService = jwtService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            try
            {
                var user = await _authService.LoginAsync(loginUser.login, loginUser.password);

                string token = _jwtService.GenerateToken(user);
                return Ok(new { token });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO userDTO)
        {
            try
            {
                var newUser = await _authService.RegisterAsync(userDTO.Login, userDTO.Password, userDTO.Email);

                string token = _jwtService.GenerateToken(newUser);
                return Ok(new { token });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("change_password")]
        public async Task<IActionResult> ChangePassword(Models.DTOs.User.Request.UpdateUserDTO userDTO)
        {
            var userIdClaim = HttpContext.User.FindFirstValue("sub");
            int userId = int.Parse(userIdClaim);

            try
            {
                await _authService.ChangePasswordAsync(userId, userDTO.Password);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("link_email")]
        public async Task<IActionResult> LinkEmail(Models.DTOs.User.Request.UpdateUserDTO userDTO)
        {
            var userIdClaim = HttpContext.User.FindFirstValue("sub");
            int userId = int.Parse(userIdClaim);

            try
            {
                await _authService.LinkEmailAsync(userId, userDTO.Email);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
