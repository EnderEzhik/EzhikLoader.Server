using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using EzhikLoader.Server.Services;

namespace EzhikLoader.Server.Controllers.User
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UsersController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userIdClaim = HttpContext.User.FindFirstValue("sub");

            int userId = int.Parse(userIdClaim);

            var user = await _userService.GetUserByIDAsync(userId);
            
            return Ok(user);
        }
    }
}
