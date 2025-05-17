using Microsoft.AspNetCore.Mvc;

namespace EzhikLoader.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> User()
        {
            return Ok();
        }
    }
}
