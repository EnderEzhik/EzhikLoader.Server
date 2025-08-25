using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Server.Services;

namespace EzhikLoader.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userIdClaim = HttpContext.User.FindFirstValue("sub");

            int userId = int.Parse(userIdClaim);

            var user = await _userService.GetUserByIdAsync(userId);
            
            return Ok(user);
        }
    }
}
