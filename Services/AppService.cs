using Microsoft.EntityFrameworkCore;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Services
{
    public class AppService
    {
        private readonly MyDbContext _dbContext;
        private readonly string _appsFilesDirectory;

        public AppService(MyDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _appsFilesDirectory = config["AppsFilesDirectory"]!;
        }

        public async Task<List<App>> GetAllApps()
        {
            var apps = await _dbContext.Apps.Where(a => a.IsActive == true).ToListAsync();
            return apps;
        }

        public async Task<List<App>> GetAvailableApps(int userId)
        {
            var apps = await _dbContext.Subscriptions.Where(s => s.UserId == userId && s.EndDate > DateTime.UtcNow)
                .Select(s => s.App).ToListAsync();
            return apps;
        }

        public async Task<(FileStream FileStream, string FileName)> GetFileApp(int appId, int userId)
        {
            var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Id == appId);

            if (app == null)
            {
                throw new FileNotFoundException($"App with ID {appId} not found");
                //return (null, null, $"App with ID {appId} not found");
            }

            var subscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId 
                                       && s.AppId == appId 
                                       && s.EndDate > DateTime.UtcNow);

            if (subscription == null)
            {
                throw new UnauthorizedAccessException("Forbidden: No active subscription");
                //return (null, null, "Forbidden: No active subscription");
            }

            subscription.LastDownloadedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            string filePath = Path.Combine(_appsFilesDirectory, app.Id.ToString(), app.FileName);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return (fileStream, app.FileName);
        }
    }
}
