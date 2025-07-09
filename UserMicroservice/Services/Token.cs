using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserMicroservice.Models;
using Microsoft.IdentityModel.Tokens;

namespace UserMicroservice.Services
{
    public class Token
    {
        private readonly IConfiguration _configuration;
        public Token(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(User existing)
        {
            // Code to set Claim
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, existing.UserName),
                new Claim(ClaimTypes.Role, existing.Role.ToString()),
            };

            // Code to generate token
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var audience = _configuration.GetSection("JWT:ValidAudiences").Get<string[]>().First();
            // Or select based on context, e.g., via parameter

            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: audience, // Must be a single string value!
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signInCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    }
}
