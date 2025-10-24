using CaseSaggezza_Dal.Contexts;
using CaseSaggezza_Domain.Descriptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CaseSaggezza_Dal.Extensions
{
    public static class MigrationExtensions
    {
        public static async void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using IdentificationDbContext contextIdentity = scope.ServiceProvider.GetRequiredService<IdentificationDbContext>();

            contextIdentity.Database.Migrate();

            using CaseSaggezzaDbContext context = scope.ServiceProvider.GetRequiredService<CaseSaggezzaDbContext>();

            context.Database.Migrate();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(Roles.Normal))
                await roleManager.CreateAsync(new IdentityRole(Roles.Normal));

            if (!await roleManager.RoleExistsAsync(Roles.Reduced))
                await roleManager.CreateAsync(new IdentityRole(Roles.Reduced));

            if (!await roleManager.RoleExistsAsync(Roles.Admin))
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }
    }
}
