using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }

    public class JwtService: IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            string jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("jwt key can not be null");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("jti", Guid.NewGuid().ToString()),
                new Claim("sub", user.Id.ToString()),
                new Claim("role", user.Role.Name)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
