using CaseSaggezza_Domain.Descriptions;
using CaseSaggezza_Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Entities = CaseSaggezza_Domain.Entities;

namespace CaseSaggezza.Services.User
{
    public class CreateJwtTokenService(UserManager<Entities.User> userManager, IConfiguration configuration)
    {
        private UserManager<Entities.User> _userManager = userManager;
        private IConfiguration _configuration = configuration;

        public async Task<string> CreateJwtToken(Entities.User user, IList<string> roles)
        {
            SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));

            SigningCredentials? credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, user.Id) };

            if (roles.Any())
                claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();

            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
