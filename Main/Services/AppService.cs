using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Models;
using EzhikLoader.Server.Models.DTOs.Admin.Request;
using EzhikLoader.Server.Models.DTOs.User.Response;
using EzhikLoader.Server.Exceptions;

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
            _appsFilesDirectory = config["AppsFilesDirectory"] ?? throw new ArgumentNullException("file storage directory not found");
        }

        public async Task<List<AppDTO>> GetAllAppsAsync()
        {
            var apps = await _dbContext.Apps.Where(a => a.IsActive == true).ToListAsync();
            return _mapper.Map<List<AppDTO>>(apps);
        }

        public async Task<List<AppDTO>> GetAvailableAppsAsync(int userId)
        {
            var apps = await _dbContext.Subscriptions.Where(s => s.UserId == userId && s.EndDate > DateTime.UtcNow && !s.IsCanceled)
                .Select(s => s.App).ToListAsync();
            return _mapper.Map<List<AppDTO>>(apps);
        }

        public async Task<AppDTO> GetAppByIdAsync(int appId)
        {
            var app = await _dbContext.Apps.FindAsync(appId);
            if (app == null)
            {
                throw new NotFoundException($"app with ID {appId} not found");
            }

            return _mapper.Map<AppDTO>(app);
        }

        public async Task<(FileStream FileStream, string FileName)> GetFileAppAsync(int appId, int userId)
        {
            var app = await _dbContext.Apps.FindAsync(appId);
            if (app == null)
            {
                throw new NotFoundException($"app with ID {appId} not found");
            }

            if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
            {
                throw new NotFoundException($"user with ID {userId} not found");
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

        public async Task<AppDTO> CreateAppAsync(CreateAppDTO appDTO)
        {
            if (await _dbContext.Apps.AnyAsync(a => a.Name == appDTO.Name))
            {
                throw new BadRequestException($"app with Name \"{appDTO.Name}\" already exist");
            }

            App newApp = new App();

            _mapper.Map(appDTO, newApp);

            _dbContext.Apps.Add(newApp);
            await _dbContext.SaveChangesAsync();

            string filePath = Path.Combine(_appsFilesDirectory, newApp.Id.ToString(), newApp.FileName);
            Directory.CreateDirectory(Path.Combine(_appsFilesDirectory, newApp.Id.ToString()));
            using (FileStream fs = new FileStream(filePath, FileMode.CreateNew))
            {
                await appDTO.File.CopyToAsync(fs);
            }

            return _mapper.Map<AppDTO>(newApp);
        }

        public async Task<AppDTO> DeleteAppAsync(int appId)
        {
            var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Id == appId);

            if (app == null)
            {
                throw new NotFoundException($"app with ID {appId} not found");
            }

            _dbContext.Apps.Remove(app);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AppDTO>(app);
        }

        //TODO: добавить проверку пробелов в строках
        public async Task UpdateAppAsync(UpdateAppDTO appDTO)
        {
            var app = await _dbContext.Apps.FindAsync(appDTO.Id);
            if (app == null)
            {
                throw new NotFoundException($"app with ID {appDTO.Id} not found");
            }

            _mapper.Map(appDTO, app);

            await _dbContext.SaveChangesAsync();
        }
    }
}
