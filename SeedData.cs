using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreTodo
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            IServiceProvider services)
        {
            var roleManager = services
                .GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services
                .GetRequiredService<UserManager<IdentityUser>>();
            await EnsureTestAdminAsync(userManager);
        }

        private static async Task EnsureRolesAsync(
        RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager
                .RoleExistsAsync(Constants.AdministratorRole);

            if (alreadyExists) return;

            await roleManager.CreateAsync(
                new IdentityRole(Constants.AdministratorRole));
        }

        private static async Task EnsureTestAdminAsync(
        UserManager<IdentityUser> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "adminFacu@todo.local")
                .SingleOrDefaultAsync();

            if (testAdmin != null) return;

            testAdmin = new IdentityUser
            {
                UserName = "adminFacu@todo.local",
                Email = "adminFacu@todo.local"
            };
            await userManager.CreateAsync(
                testAdmin, "Facundo123_");
            await userManager.AddToRoleAsync(
                testAdmin, Constants.AdministratorRole);
        }

        public static class Constants
        {
            public const string AdministratorRole = "Administrator";
        }
    }
}