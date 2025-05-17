using Microsoft.AspNetCore.Mvc;

namespace EzhikLoader.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppsController : ControllerBase
    {
        public AppsController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> AllApps()
        {
            return Ok();
        }

        [HttpGet("available")]
        public async Task<IActionResult> AvailableApp()
        {
            return Ok();
        }

        [HttpGet("/{id}/file")]
        public async Task<IActionResult> FileApp(int id)
        {
            return Ok();
        }
    }
}
