using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Admin.Models.User;
using EzhikLoader.Admin.Services;
using EzhikLoader.Admin.Exceptions;

namespace EzhikLoader.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService userService;

        public UsersController(UserService _userService)
        {
            userService = _userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUser(int id)
        {
            var user = await userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUserRequestDTO user)
        {
            var updatedUser = await userService.UpdateUserAsync(id, user);
            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequestDTO createUser)
        {
            try
            {
                var user = await userService.CreateUserAsync(createUser);
                return CreatedAtAction(
                    nameof(Controllers.UsersController.GetUser),
                    new { id = user.Id },
                    user);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool deleted = await userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
