using Devella.DataAccessLayer.Data;
using Devella.DataAccessLayer.Enums;
using Devella.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Devella.DataAccessLayer.Services;

public static class DbSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {

        foreach (var role in Enum.GetValues(typeof(Role)).Cast<Role>())
        {
            var roleName = role.ToString();
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public static async Task SeedUsersAsync(UserManager<User> userManager, ApplicationDbContext context)
    {
        await SeedDeveloperAsync(userManager, context);
        await SeedClientAsync(userManager, context);
        await SeedAdminAsync(userManager, context);
    }

    private static async Task SeedDeveloperAsync(UserManager<User> userManager, ApplicationDbContext context)
    {
        var email = "developer@example.com";
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                FirstName = "Developer",
                Surname = "Andersson",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "DeveloperPass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Role.Developer.ToString());

                var developerProfile = new DeveloperUser
                {
                    UserId = user.Id,
                    Competence = null,
                    Experience = 3,
                    School = "KTH",
                    WantedPosition = TypeOfPosition.FullTime,
                    Description = "Seeded developer"
                };

                context.DeveloperUsers.Add(developerProfile);
                await context.SaveChangesAsync();
            }
        }
    }

    private static async Task SeedClientAsync(UserManager<User> userManager, ApplicationDbContext context)
    {
        var email = "client@example.com";
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                FirstName = "Client",
                Surname = "Bengtsson",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "ClientPass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Role.Client.ToString());

                var companyProfile = new CompanyUser
                {
                    UserId = user.Id,
                    CompanyName = "Seeded Co",
                    SavedDevelopers = new List<DeveloperUser>()
                };

                context.CompanyUsers.Add(companyProfile);
                await context.SaveChangesAsync();
            }
        }
    }

    private static async Task SeedAdminAsync(UserManager<User> userManager, ApplicationDbContext context)
    {
        var email = "admin@example.com";
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                FirstName = "Admin",
                Surname = "Adminsson",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "AdminPass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Role.Admin.ToString());

                var adminProfile = new AdminUser
                {
                    UserId = user.Id,
                };

                context.AdminUsers.Add(adminProfile);
                await context.SaveChangesAsync();
            }
        }
    }
}

