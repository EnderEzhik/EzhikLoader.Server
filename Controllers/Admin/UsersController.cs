using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EzhikLoader.Server.Services;
using EzhikLoader.Server.Models.DTOs.Admin.Request;

namespace EzhikLoader.Server.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync(CreateUserDTO createUser)
        {
            try
            {
                var user = await _userService.CreateUserAsync(createUser);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserByIDAsync(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIDAsync(userId);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserDTO updateUser)
        {
            try
            {
                await _userService.UpdateUserAsync(updateUser);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _userService.DeleteUserByIDAsync(userId);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
