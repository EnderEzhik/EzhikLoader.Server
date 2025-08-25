using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EzhikLoader.Server.Services;
using EzhikLoader.Server.Models.DTOs.Admin.Request;
using EzhikLoader.Server.Models.DTOs.User.Response;
using EzhikLoader.Server.Exceptions;

namespace EzhikLoader.Server.Controllers.Admin
{
    [ApiController]
    [Route("api/apps")]
    [Authorize(Roles = "admin")]
    public class AppsController : ControllerBase
    {
        private readonly AppService _appService;

        public AppsController(AppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateApp(CreateAppDTO appDTO)
        {
            try
            {
                AppDTO app = await _appService.CreateAppAsync(appDTO);
                return CreatedAtAction(
                    nameof(Controllers.User.AppsController.GetAppById),
                    new { appId = app.Id },
                    app);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateApp(UpdateAppDTO updateApp)
        {
            try
            {
                await _appService.UpdateAppAsync(updateApp);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{appId}")]
        public async Task<IActionResult> DeleteApp(int appId)
        {
            try
            {
                var app = await _appService.DeleteAppAsync(appId);
                return Ok(app);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
