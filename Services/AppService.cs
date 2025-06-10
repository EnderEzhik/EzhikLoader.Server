using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models;
using EzhikLoader.Server.Models.DTOs.Admin.Request;
using AutoMapper;

namespace EzhikLoader.Server.Services
{
    public class AppService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly string _appsFilesDirectory;

        public AppService(MyDbContext dbContext, IMapper mapper, IConfiguration config)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _appsFilesDirectory = config["AppsFilesDirectory"]!;
        }

        public async Task<List<Models.DTOs.User.Response.AppDTO>> GetAllAppsAsync()
        {
            var apps = await _dbContext.Apps.Where(a => a.IsActive == true).ToListAsync();
            return _mapper.Map<List<Models.DTOs.User.Response.AppDTO>>(apps);
        }

        public async Task<List<Models.DTOs.User.Response.AppDTO>> GetAvailableAppsAsync(int userId)
        {
            var apps = await _dbContext.Subscriptions.Where(s => s.UserId == userId && s.EndDate > DateTime.UtcNow)
                .Select(s => s.App).ToListAsync();
            return _mapper.Map<List<Models.DTOs.User.Response.AppDTO>>(apps);
        }

        public async Task<(FileStream FileStream, string FileName)> GetFileAppAsync(int appId, int userId)
        {
            var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Id == appId);
            if (app == null)
            {
                throw new ArgumentException($"app with ID {appId} not found");
            }

            if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
            {
                throw new ArgumentException($"user with ID {userId} not found");
            }

            var subscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId
                                       && s.AppId == appId
                                       && s.EndDate > DateTime.UtcNow);

            if (subscription == null)
            {
                throw new UnauthorizedAccessException();
            }

            string filePath = Path.Combine(_appsFilesDirectory, app.Id.ToString(), app.FileName);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            subscription.LastDownloadedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return (fileStream, app.FileName);
        }

        public async Task<Models.DTOs.User.Response.AppDTO> CreateAppAsync(CreateAppDTO newAppDTO)
        {
            var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Name == newAppDTO.Name);
            if (app != null)
            {
                throw new ArgumentException($"app with Name \"{newAppDTO.Name}\" already exist");
            }

            App newApp = new App()
            {
                Name = newAppDTO.Name,
                Description = newAppDTO.Description,
                Version = newAppDTO.Version,
                Price = newAppDTO.Price!.Value,
                IsActive = newAppDTO.IsActive!.Value,
                FileName = newAppDTO.FileName,
                IconName = newAppDTO.IconName
            };

            _dbContext.Apps.Add(newApp);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Models.DTOs.User.Response.AppDTO>(newApp);
        }

        public async Task<Models.DTOs.User.Response.AppDTO> DeleteAppAsync(int appId)
        {
            var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Id == appId);

            if (app == null)
            {
                throw new ArgumentException($"app with ID {appId} not found");
            }

            _dbContext.Apps.Remove(app);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Models.DTOs.User.Response.AppDTO>(app);
        }

        public async Task UpdateAppAsync(UpdateAppDataDTO updateApp)
        {
            var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Id == updateApp.Id);
            if (app == null)
            {
                throw new ArgumentException($"app with ID {updateApp.Id} not found");
            }

            if (updateApp.Name != null)
            {
                app.Name = updateApp.Name;
            }
            if (updateApp.Description != null)
            {
                app.Description = updateApp.Description;
            }
            if (updateApp.Version != null)
            {
                app.Version = updateApp.Version;
            }
            if (updateApp.Price.HasValue)
            {
                app.Price = updateApp.Price.Value;
            }
            if (updateApp.IsActive.HasValue)
            {
                app.IsActive = updateApp.IsActive.Value;
            }
            if (updateApp.FileName != null)
            {
                app.FileName = updateApp.FileName;
            }
            if (updateApp.IconName != null)
            {
                app.IconName = updateApp.IconName;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
