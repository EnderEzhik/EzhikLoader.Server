using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Admin.Services;
using EzhikLoader.Admin.Models.App;

namespace EzhikLoader.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppsController : ControllerBase
    {
        private readonly AppService appService;

        public AppsController(AppService _appService)
        {
            appService = _appService;
        }

        // GET: api/apps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppResponseDTO>>> GetApps()
        {
            var apps = await appService.GetAllAppsAsync();
            return Ok(apps);
        }

        // GET: api/apps/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<AppResponseDTO>>> GetActiveApps()
        {
            var apps = await appService.GetActiveAppsAsync();
            return Ok(apps);
        }

        // GET: api/apps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppResponseDTO>> GetApp(int id)
        {
            var app = await appService.GetAppByIdAsync(id);

            if (app == null)
            {
                return NotFound();
            }

            return app;
        }

        // POST: api/apps
        [HttpPost]
        public async Task<IActionResult> CreateApp(CreateAppRequestDTO createApp)
        {
            try
            {
                var app = await appService.CreateAppAsync(createApp);
                return CreatedAtAction(
                    nameof(GetApp),
                    new { id = app.Id },
                    app);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/apps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApp(int id, UpdateAppRequestDTO appUpdate)
        {
            try
            {
                var updatedApp = await appService.UpdateAppAsync(id, appUpdate);
                if (updatedApp == null)
                {
                    return NotFound();
                }

                return Ok(updatedApp);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/apps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApp(int id)
        {
            try
            {
                bool deleted = await appService.DeleteAppAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/apps/5/activate
        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateApp(int id)
        {
            var app = await appService.ActivateAppAsync(id);
            if (app == null)
            {
                return NotFound();
            }

            return Ok(app);
        }

        // POST: api/apps/5/deactivate
        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateApp(int id)
        {
            var app = await appService.DeactivateAppAsync(id);
            if (app == null)
            {
                return NotFound();
            }

            return Ok(app);
        }
    }
}
