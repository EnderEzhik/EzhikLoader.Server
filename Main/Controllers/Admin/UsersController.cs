using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EzhikLoader.Server.Services;
using EzhikLoader.Server.Models.DTOs.Admin.Request;
using EzhikLoader.Server.Exceptions;

namespace EzhikLoader.Server.Controllers.Admin
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(CreateUserDTO createUser)
        {
            try
            {
                var user = await _userService.CreateUserAsync(createUser);
                return CreatedAtAction(
                    nameof(Controllers.Admin.UsersController.GetUserById),
                    new { userId=user.Id },
                    user);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var user = await _userService.GetDetailedUserByIdAsync(userId);
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserDTO updateUser)
        {
            try
            {
                await _userService.UpdateUserAsync(updateUser);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _userService.DeleteUserByIdAsync(userId);
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
