using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models.DTOs.Response;

namespace EzhikLoader.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppsController : ControllerBase
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly string appsFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "AppsFiles");

        public AppsController(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

            if (!Directory.Exists(appsFilesDirectory))
            {
                Directory.CreateDirectory(appsFilesDirectory);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllApps()
        {
            var apps = await _dbContext.Apps.Where(a => a.IsActive == true).ToListAsync();

            return Ok(_mapper.Map<List<AppDTO>>(apps));
        }

        [HttpGet("available")]
        public async Task<IActionResult> AvailableApp()
        {
            var userIdClaim = HttpContext.User.FindFirst("sub")!.Value;

            var userId = int.Parse(userIdClaim);

            var availableApps = await _dbContext.Subscriptions
                .Where(s => s.UserId == userId && s.EndDate > DateTime.UtcNow)
                .Select(s => s.App).ToListAsync();

            return Ok(_mapper.Map<List<AppDTO>>(availableApps));
        }

        [HttpGet("{appId}/file")]
        public async Task<IActionResult> FileApp(int appId)
        {
            var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Id == appId);

            if (app == null)
            {
                return BadRequest($"App with ID {appId} not found");
            }

            var userIdClaim = HttpContext.User.FindFirst("sub")!.Value;

            var userId = int.Parse(userIdClaim);

            var subscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId 
                && s.Id == appId 
                && s.EndDate > DateTime.UtcNow);

            if (subscription == null)
            {
                return Forbid();
            }

            string filePath = Path.Combine(appsFilesDirectory, app.Id.ToString(), app.FileName);

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/octet-stream", app.FileName);
        }
    }
}
