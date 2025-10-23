using CaseSaggezza.Services.User;
using CaseSaggezza_Dal.Contexts;
using CaseSaggezza_Domain.Descriptions;
using CaseSaggezza_Domain.Dto;
using CaseSaggezza_Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
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
            RegisterService registerService = new RegisterService(_userManager);

            using var transaction = await _identificationContext.Database.BeginTransactionAsync();

            if (await registerService.CheckEmail(registerRequest.Email) != null)
                return BadRequest("Email já registrado");

            User user = registerService.CreateUserObject(registerRequest);

            IdentityResult result = await registerService.CreateUser(user, registerRequest);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            IdentityResult addRoleUser = await registerService.CreateRoles(user, Roles.Normal);

            if (!addRoleUser.Succeeded)
                return BadRequest(result.Errors);

            await transaction.CommitAsync();

            return Ok("Registrado com sucesso!");
        }

        [HttpPost("Login")]
        public async Task<IResult> Login(LoginRequestDto loginRequest)
        {
            RegisterService registerService = new RegisterService(_userManager);
            LoginService loginService = new LoginService(_userManager);
            CreateJwtTokenService createJwtTokenService = new CreateJwtTokenService(_userManager, _configuration);

            User? user = await registerService.CheckEmail(loginRequest.Email);

            if (user == null || !await loginService.CheckPassword(user, loginRequest))
                return Results.Unauthorized();

            IList<string> roles = await loginService.UserRoles(user);

            string token = await createJwtTokenService.CreateJwtToken(user, roles);

            return Results.Ok(new {token, user.UserName});
        }
    }
}
