using CaseSaggezza_Dal.Contexts;
using CaseSaggezza_Domain.Descriptions;
using CaseSaggezza_Domain.Dto;
using CaseSaggezza_Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace CaseSaggezza.Controllers
{
    [Route("Api/[controller]")]
    public class UserController(IdentificationDbContext identificationContext, UserManager<User> userManager, IConfiguration configuration) : ControllerBase
    {
        private IdentificationDbContext _identificationContext = identificationContext;
        private UserManager<User> _userManager = userManager;
        private IConfiguration _configuration = configuration;

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequest)
        {
            using var transaction = await _identificationContext.Database.BeginTransactionAsync();

            if (await _userManager.FindByNameAsync(registerRequest.Email) != null)
                return BadRequest("Email já registrado");

            User user = new User
            {
                UserName = registerRequest.Email,
                Name = registerRequest.Name,
                Email = registerRequest.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            IdentityResult addRoleUser = await _userManager.AddToRoleAsync(user, Roles.Normal);

            if (!addRoleUser.Succeeded)
                return BadRequest(result.Errors);

            await transaction.CommitAsync();

            return Ok("Registrado com sucesso!");
        }

        [HttpPost("Login")]
        public async Task<IResult> Login(LoginRequestDto loginRequest)
        {
            User? user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                return Results.Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

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

            string token = tokenHandler.CreateToken(tokenDescriptor);

            return Results.Ok(new {token, user.UserName});
        }
    }
}
