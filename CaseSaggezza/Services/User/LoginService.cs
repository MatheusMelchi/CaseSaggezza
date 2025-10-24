using Microsoft.AspNetCore.Identity;
using CaseSaggezza_Domain.Entities;
using Entities = CaseSaggezza_Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;
using CaseSaggezza_Domain.Dto;

namespace CaseSaggezza.Services.User
{
    public class LoginService(UserManager<Entities.User> userManager)
    {
        private UserManager<Entities.User> _userManager = userManager;

        public async Task<bool> CheckPassword(Entities.User user, LoginRequestDto request)
        {
            return await _userManager.CheckPasswordAsync(user, request.Password);
        }

        public async Task<IList<string>> UserRoles(Entities.User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
