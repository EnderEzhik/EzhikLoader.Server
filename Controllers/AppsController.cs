using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using EzhikLoader.Server.Services;
using EzhikLoader.Server.Models.DTOs.Response;

namespace EzhikLoader.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppService _appService;

        public AppsController(IMapper mapper, AppService appService)
        {
            _mapper = mapper;
            _appService = appService;
        }

        [HttpGet]
        public async Task<IActionResult> AllApps()
        {
            var apps = await _appService.GetAllApps();

            return Ok(_mapper.Map<List<AppDTO>>(apps));
        }

        [HttpGet("available")]
        public async Task<IActionResult> AvailableApp()
        {
            var userIdClaim = HttpContext.User.FindFirst("sub")!.Value;

            var userId = int.Parse(userIdClaim);

            var availableApps = await _appService.GetAvailableApps(userId);

            return Ok(_mapper.Map<List<AppDTO>>(availableApps));
        }

        [HttpGet("{appId}/file")]
        public async Task<IActionResult> FileApp(int appId)
        {
            string userIdClaim = HttpContext.User.FindFirstValue("sub");
            var userId = int.Parse(userIdClaim);

            try
            {
                var (fileStream, fileName) = await _appService.GetFileApp(appId, userId);

                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (FileNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}
