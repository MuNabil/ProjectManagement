using DataAccess_EF.Data;
using Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace DataAccess_EF.Extensions
{
    public static class Seed
    {
        public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var basicUser = new ApplicationUser
            {
                FirstName = "basic",
                LastName = "user",
                UserName = "basic@test.com",
                Email = "basic@test.com",
                EmailConfirmed = true,
            };
            var user = await userManager.FindByEmailAsync(basicUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(basicUser, "P@ssword123");
                await userManager.AddToRoleAsync(basicUser, Roles.User.ToString());
            }

            // Seed Admin
            var adminUser = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "user",
                UserName = "admin@test.com",
                Email = "admin@test.com",
                EmailConfirmed = true
            };
            user = await userManager.FindByEmailAsync(adminUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(adminUser, "P@ssword123");
                await userManager.AddToRolesAsync(adminUser, new List<string>
                {
                    Roles.Admin.ToString(),
                    Roles.User.ToString()
                });
            }
        }
    }
}
