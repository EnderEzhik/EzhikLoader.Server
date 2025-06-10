using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EzhikLoader.Server.Logger;
using EzhikLoader.Server.Data;
using EzhikLoader.Server.Interfaces;
using EzhikLoader.Server.Services;

namespace EzhikLoader.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddDbContext<MyDbContext>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    NameClaimType = "sub",
                    RoleClaimType = "role"
                };

                options.MapInboundClaims = false;
            });
            builder.Services.AddAuthorization();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<AppService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<SubscriptionService>();
            builder.Services.AddScoped<IPaymentService, FakePaymentService>();
            builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logs.txt"));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            app.UseLoggerMiddleware(app.Logger);

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
