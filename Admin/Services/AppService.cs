using AutoMapper;
using EzhikLoader.Admin.Database;
using EzhikLoader.Admin.Entities;
using EzhikLoader.Admin.Models.App;
using Microsoft.EntityFrameworkCore;

namespace EzhikLoader.Admin.Services
{
    public class AppService
    {
        private readonly MyDbContext db;
        private readonly IMapper mapper;

        public AppService(MyDbContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }

        /// <summary>
        /// Создает новое приложение
        /// </summary>
        public async Task<AppResponseDTO> CreateAppAsync(CreateAppRequestDTO appDTO)
        {
            // Проверяем, не существует ли уже приложение с таким именем
            bool nameExists = await db.Apps.AnyAsync(a => a.Name == appDTO.Name);
            if (nameExists)
            {
                throw new InvalidOperationException($"app with name \"{appDTO.Name}\" is already exist");
            }

            // Проверяем, не существует ли уже приложение с таким именем файла
            // bool fileNameExists = await db.Apps.AnyAsync(a => a.FileName == appDTO.FileName);
            // if (fileNameExists)
            // {
            //     throw new InvalidOperationException($"Приложение с именем файла \"{appDTO.FileName}\" уже существует");
            // }

            var app = mapper.Map<App>(appDTO);
            app.CreatedAt = DateTime.UtcNow;
            app.LastUpdatedAt = DateTime.UtcNow;
            app.IsActive = true;

            db.Apps.Add(app);
            await db.SaveChangesAsync();

            return mapper.Map<AppResponseDTO>(app);
        }

        /// <summary>
        /// Получает приложение по ID
        /// </summary>
        public async Task<AppResponseDTO?> GetAppByIdAsync(int id)
        {
            var app = await db.Apps.FindAsync(id);
            if (app == null)
            {
                return null;
            }
            return mapper.Map<AppResponseDTO>(app);
        }

        /// <summary>
        /// Получает все приложения
        /// </summary>
        public async Task<IEnumerable<AppResponseDTO>> GetAllAppsAsync()
        {
            var apps = await db.Apps.ToListAsync();
            return apps.Select(a => mapper.Map<AppResponseDTO>(a));
        }

        /// <summary>
        /// Получает только активные приложения
        /// </summary>
        public async Task<IEnumerable<AppResponseDTO>> GetActiveAppsAsync()
        {
            var apps = await db.Apps.Where(a => a.IsActive).ToListAsync();
            return apps.Select(a => mapper.Map<AppResponseDTO>(a));
        }

        /// <summary>
        /// Обновляет приложение
        /// </summary>
        public async Task<AppResponseDTO?> UpdateAppAsync(int id, UpdateAppRequestDTO appUpdate)
        {
            var app = await db.Apps.FindAsync(id);
            if (app == null)
            {
                return null;
            }

            // Проверяем уникальность имени, если оно передается
            if (appUpdate.Name != null && appUpdate.Name != app.Name)
            {
                bool nameExists = await db.Apps.AnyAsync(a => a.Name == appUpdate.Name && a.Id != id);
                if (nameExists)
                {
                    throw new InvalidOperationException($"Приложение с названием \"{appUpdate.Name}\" уже существует");
                }
            }

            // Проверяем уникальность имени файла, если оно передается
            if (appUpdate.FileName != null && appUpdate.FileName != app.FileName)
            {
                bool fileNameExists = await db.Apps.AnyAsync(a => a.FileName == appUpdate.FileName && a.Id != id);
                if (fileNameExists)
                {
                    throw new InvalidOperationException($"Приложение с именем файла \"{appUpdate.FileName}\" уже существует");
                }
            }

            // Обновляем только те поля, которые действительно переданы
            if (appUpdate.Name != null)
            {
                app.Name = appUpdate.Name;
            }

            if (appUpdate.Description != null)
            {
                app.Description = appUpdate.Description;
            }

            if (appUpdate.Version != null)
            {
                app.Version = appUpdate.Version;
            }

            if (appUpdate.Price.HasValue)
            {
                app.Price = appUpdate.Price.Value;
            }

            if (appUpdate.IsActive.HasValue)
            {
                app.IsActive = appUpdate.IsActive.Value;
            }

            if (appUpdate.FileName != null)
            {
                app.FileName = appUpdate.FileName;
            }

            if (appUpdate.IconName != null)
            {
                app.IconName = appUpdate.IconName;
            }

            app.LastUpdatedAt = DateTime.UtcNow;

            db.Entry(app).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return mapper.Map<AppResponseDTO>(app);
        }

        /// <summary>
        /// Удаляет приложение
        /// </summary>
        public async Task<bool> DeleteAppAsync(int id)
        {
            var app = await db.Apps.FindAsync(id);
            if (app == null)
            {
                return false;
            }

            // Проверяем, есть ли активные подписки на это приложение
            // Подписка считается активной, если она не отменена и не истекла
            bool hasActiveSubscriptions = await db.Subscriptions.AnyAsync(s =>
                s.AppId == id &&
                !s.IsCanceled &&
                s.EndDate > DateTime.UtcNow);
            if (hasActiveSubscriptions)
            {
                throw new InvalidOperationException("Нельзя удалить приложение с активными подписками");
            }

            db.Apps.Remove(app);
            await db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Активирует приложение
        /// </summary>
        public async Task<AppResponseDTO?> ActivateAppAsync(int id)
        {
            var app = await db.Apps.FindAsync(id);
            if (app == null)
            {
                return null;
            }

            app.IsActive = true;
            app.LastUpdatedAt = DateTime.UtcNow;

            db.Entry(app).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return mapper.Map<AppResponseDTO>(app);
        }

        /// <summary>
        /// Деактивирует приложение
        /// </summary>
        public async Task<AppResponseDTO?> DeactivateAppAsync(int id)
        {
            var app = await db.Apps.FindAsync(id);
            if (app == null)
            {
                return null;
            }

            app.IsActive = false;
            app.LastUpdatedAt = DateTime.UtcNow;

            db.Entry(app).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return mapper.Map<AppResponseDTO>(app);
        }
    }
}
