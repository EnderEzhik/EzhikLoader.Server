using AutoMapper;
using EzhikLoader.Admin.Database;
using EzhikLoader.Admin.Entities;
using EzhikLoader.Admin.Models.Subscription;
using Microsoft.EntityFrameworkCore;

namespace EzhikLoader.Admin.Services
{
    public class SubscriptionService
    {
        private readonly MyDbContext db;
        private readonly IMapper mapper;

        public SubscriptionService(MyDbContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }

        /// <summary>
        /// Создает новую подписку
        /// </summary>
        public async Task<SubscriptionResponseDTO> CreateSubscriptionAsync(CreateSubscriptionRequestDTO subscriptionDTO)
        {
            // Проверяем, существует ли пользователь
            var user = await db.Users.FindAsync(subscriptionDTO.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {subscriptionDTO.UserId} not found");
            }

            // Проверяем, существует ли приложение
            var app = await db.Apps.FindAsync(subscriptionDTO.AppId);
            if (app == null)
            {
                throw new InvalidOperationException($"App with ID {subscriptionDTO.AppId} not found");
            }

            // Проверяем, существует ли платеж
            var payment = await db.Payments.FindAsync(subscriptionDTO.PaymentId);
            if (payment == null)
            {
                throw new InvalidOperationException($"Payment with ID {subscriptionDTO.PaymentId} not found");
            }

            // Проверяем, нет ли уже активной подписки у пользователя на это приложение
            bool hasActiveSubscription = await db.Subscriptions.AnyAsync(s =>
                s.UserId == subscriptionDTO.UserId &&
                s.AppId == subscriptionDTO.AppId &&
                !s.IsCanceled &&
                s.EndDate > DateTime.UtcNow);

            if (hasActiveSubscription)
            {
                throw new InvalidOperationException($"User already has an active subscription to this app");
            }

            var subscription = mapper.Map<Subscription>(subscriptionDTO);
            subscription.IsCanceled = false; // По умолчанию подписка не отменена

            db.Subscriptions.Add(subscription);
            await db.SaveChangesAsync();

            // Загружаем связанные данные для маппинга в DTO
            await db.Entry(subscription).Reference(s => s.User).LoadAsync();
            await db.Entry(subscription).Reference(s => s.App).LoadAsync();

            return mapper.Map<SubscriptionResponseDTO>(subscription);
        }

        /// <summary>
        /// Получает подписку по ID
        /// </summary>
        public async Task<SubscriptionResponseDTO?> GetSubscriptionByIdAsync(int id)
        {
            var subscription = await db.Subscriptions
                .Include(s => s.User)
                .Include(s => s.App)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subscription == null)
            {
                return null;
            }

            return mapper.Map<SubscriptionResponseDTO>(subscription);
        }

        /// <summary>
        /// Получает все подписки
        /// </summary>
        public async Task<IEnumerable<SubscriptionResponseDTO>> GetAllSubscriptionsAsync()
        {
            var subscriptions = await db.Subscriptions
                .Include(s => s.User)
                .Include(s => s.App)
                .ToListAsync();

            return subscriptions.Select(s => mapper.Map<SubscriptionResponseDTO>(s));
        }

        /// <summary>
        /// Получает подписки пользователя
        /// </summary>
        public async Task<IEnumerable<SubscriptionResponseDTO>> GetUserSubscriptionsAsync(int userId)
        {
            var subscriptions = await db.Subscriptions
                .Include(s => s.User)
                .Include(s => s.App)
                .Where(s => s.UserId == userId)
                .ToListAsync();

            return subscriptions.Select(s => mapper.Map<SubscriptionResponseDTO>(s));
        }

        /// <summary>
        /// Получает активные подписки пользователя
        /// </summary>
        public async Task<IEnumerable<SubscriptionResponseDTO>> GetUserActiveSubscriptionsAsync(int userId)
        {
            var subscriptions = await db.Subscriptions
                .Include(s => s.User)
                .Include(s => s.App)
                .Where(s => s.UserId == userId && !s.IsCanceled && s.EndDate > DateTime.UtcNow)
                .ToListAsync();

            return subscriptions.Select(s => mapper.Map<SubscriptionResponseDTO>(s));
        }

        /// <summary>
        /// Получает подписки на приложение
        /// </summary>
        public async Task<IEnumerable<SubscriptionResponseDTO>> GetAppSubscriptionsAsync(int appId)
        {
            var subscriptions = await db.Subscriptions
                .Include(s => s.User)
                .Include(s => s.App)
                .Where(s => s.AppId == appId)
                .ToListAsync();

            return subscriptions.Select(s => mapper.Map<SubscriptionResponseDTO>(s));
        }

        /// <summary>
        /// Получает активные подписки на приложение
        /// </summary>
        public async Task<IEnumerable<SubscriptionResponseDTO>> GetAppActiveSubscriptionsAsync(int appId)
        {
            var subscriptions = await db.Subscriptions
                .Include(s => s.User)
                .Include(s => s.App)
                .Where(s => s.AppId == appId && !s.IsCanceled && s.EndDate > DateTime.UtcNow)
                .ToListAsync();

            return subscriptions.Select(s => mapper.Map<SubscriptionResponseDTO>(s));
        }

        /// <summary>
        /// Обновляет подписку
        /// </summary>
        public async Task<SubscriptionResponseDTO?> UpdateSubscriptionAsync(int id, UpdateSubscriptionRequestDTO subscriptionUpdate)
        {
            var subscription = await db.Subscriptions
                .Include(s => s.User)
                .Include(s => s.App)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subscription == null)
            {
                return null;
            }

            // Обновляем только те поля, которые действительно переданы
            if (subscriptionUpdate.StartDate.HasValue)
            {
                subscription.StartDate = subscriptionUpdate.StartDate.Value;
            }

            if (subscriptionUpdate.EndDate.HasValue)
            {
                subscription.EndDate = subscriptionUpdate.EndDate.Value;
            }

            if (subscriptionUpdate.LastDownloadedAt.HasValue)
            {
                subscription.LastDownloadedAt = subscriptionUpdate.LastDownloadedAt.Value;
            }

            if (subscriptionUpdate.IsCanceled.HasValue)
            {
                subscription.IsCanceled = subscriptionUpdate.IsCanceled.Value;
            }

            db.Entry(subscription).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return mapper.Map<SubscriptionResponseDTO>(subscription);
        }

        /// <summary>
        /// Удаляет подписку
        /// </summary>
        public async Task<bool> DeleteSubscriptionAsync(int id)
        {
            var subscription = await db.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return false;
            }

            db.Subscriptions.Remove(subscription);
            await db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Отменяет подписку
        /// </summary>
        public async Task<SubscriptionResponseDTO?> CancelSubscriptionAsync(int id)
        {
            var subscription = await db.Subscriptions
                .Include(s => s.User)
                .Include(s => s.App)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subscription == null)
            {
                return null;
            }

            subscription.IsCanceled = true;
            db.Entry(subscription).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return mapper.Map<SubscriptionResponseDTO>(subscription);
        }

        /// <summary>
        /// Возобновляет подписку
        /// </summary>
        public async Task<SubscriptionResponseDTO?> ReactivateSubscriptionAsync(int id)
        {
            var subscription = await db.Subscriptions
                .Include(s => s.User)
                .Include(s => s.App)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subscription == null)
            {
                return null;
            }

            subscription.IsCanceled = false;
            db.Entry(subscription).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return mapper.Map<SubscriptionResponseDTO>(subscription);
        }

        /// <summary>
        /// Обновляет дату последней загрузки
        /// </summary>
        public async Task<SubscriptionResponseDTO?> UpdateLastDownloadedAsync(int id)
        {
            var subscription = await db.Subscriptions
                .Include(s => s.User)
                .Include(s => s.App)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subscription == null)
            {
                return null;
            }

            subscription.LastDownloadedAt = DateTime.UtcNow;
            db.Entry(subscription).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return mapper.Map<SubscriptionResponseDTO>(subscription);
        }
    }
}
