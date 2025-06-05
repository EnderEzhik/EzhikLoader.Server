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

            app.Use(async (context, next) =>
            {
                var requestLogText = new StringBuilder();
                requestLogText.AppendLine($"Method: {context.Request.Method}");
                requestLogText.AppendLine($"Path: {context.Request.Path}");
                requestLogText.AppendLine($"User-Agent: {context.Request.Headers.UserAgent}");
                requestLogText.AppendLine($"Time: {DateTime.Now.ToLongTimeString()}");
                requestLogText.AppendLine($"Host: {context.Request.Host}");
                requestLogText.AppendLine($"Authorization: {context.Request.Headers.Authorization}");

                using (var s = new StreamReader(context.Request.Body))
                {
                    string body = await s.ReadToEndAsync();
                    requestLogText.AppendLine($"Body: {body.ReplaceLineEndings().Replace(" ", "")}");
                    context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body), true);
                }

                app.Logger.LogInformation(requestLogText.ToString());

                await next.Invoke();

                var responseLogText = new StringBuilder();
                responseLogText.AppendLine($"StatusCode: {context.Response.StatusCode}");

                if (context.Response.Body.CanRead)
                {
                    responseLogText.AppendLine($"ContentDisposition: {context.Response.Headers.ContentDisposition}");

                    using (var s = new StreamReader(context.Response.Body))
                    {
                        string body = await s.ReadToEndAsync();
                        responseLogText.AppendLine($"Body: {body.ReplaceLineEndings().Replace(" ", "")}");
                        context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
                    }
                }

                app.Logger.LogInformation(responseLogText.ToString());
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
