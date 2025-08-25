using Microsoft.EntityFrameworkCore;
using EzhikLoader.Admin.Database;
using EzhikLoader.Admin.Services;

namespace EzhikLoader.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found");
            builder.Services.AddDbContext<MyDbContext>(options =>
            {
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4, 5)));
            });
            builder.Services.AddScoped<UserService, UserService>();
            builder.Services.AddScoped<AppService, AppService>();
            builder.Services.AddScoped<SubscriptionService, SubscriptionService>();
            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));

            builder.Services.AddControllers();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            app.MapControllers();

            app.Run();
        }
    }
}
