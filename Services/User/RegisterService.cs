using Microsoft.AspNetCore.Identity;
using CaseSaggezza_Domain.Entities;
using Entities = CaseSaggezza_Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;
using CaseSaggezza_Domain.Dto;
using CaseSaggezza_Domain.Descriptions;

namespace CaseSaggezza.Services.User
{
    public class RegisterService(UserManager<Entities.User> userManager)
    {
        private UserManager<Entities.User> _userManager = userManager;

        public async Task<Entities.User> CheckEmail(string email)
        {
             return(await _userManager.FindByEmailAsync(email));
        }

        public Entities.User CreateUserObject(RegisterRequestDto request)
        {
            Entities.User user = new Entities.User
            {
                UserName = request.Email,
                Name = request.Name,
                Email = request.Email,
            };

            return user;
        }

        public async Task<IdentityResult> CreateUser(Entities.User user, RegisterRequestDto request)
        {
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            return result;
        }

        public async Task<IdentityResult> CreateRoles(Entities.User user, string role)
        {
            IdentityResult result = await _userManager.AddToRoleAsync(user, role);

            return result;
        }
    }
}
