using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using EzhikLoader.Server.Services;
using EzhikLoader.Server.Models.DTOs.User.Response;

namespace EzhikLoader.Server.Controllers.User
{
    [ApiController]
    [Route("api/apps")]
    [Authorize]
    public class AppsController : ControllerBase
    {
        private readonly AppService _appService;

        public AppsController(AppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppsAsync()
        {
            var apps = await _appService.GetAllAppsAsync();

            return Ok(apps);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableAppsAsync()
        {
            var userIdClaim = HttpContext.User.FindFirst("sub")!.Value;

            var userId = int.Parse(userIdClaim);

            var availableApps = await _appService.GetAvailableAppsAsync(userId);

            return Ok(availableApps);
        }

        [HttpGet("{appId}/file")]
        public async Task<IActionResult> GetFileAppAsync(int appId)
        {
            string userIdClaim = HttpContext.User.FindFirstValue("sub");
            var userId = int.Parse(userIdClaim);

            try
            {
                var (fileStream, fileName) = await _appService.GetFileAppAsync(appId, userId);

                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (FileNotFoundException)
            {
                return Problem($"file for app with ID {appId} not found");
            }
        }
    }
}
