using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EzhikLoader.Server.Services;
using EzhikLoader.Server.Models.DTOs.Admin.Request;

namespace EzhikLoader.Server.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/apps")]
    [Authorize(Roles = "admin")]
    public class AppsController : ControllerBase
    {
        private readonly AppService _appService;

        public AppsController(AppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppAsync([FromBody] CreateAppDTO createAppDTO)
        {
            try
            {
                var app = await _appService.CreateAppAsync(createAppDTO);
                return Ok(app);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppAsync(UpdateAppDataDTO updateApp)
        {
            try
            {
                await _appService.UpdateAppAsync(updateApp);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{appId}")]
        public async Task<IActionResult> DeleteAppAsync(int appId)
        {
            try
            {
                var app = await _appService.DeleteAppAsync(appId);
                return Ok(app);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
