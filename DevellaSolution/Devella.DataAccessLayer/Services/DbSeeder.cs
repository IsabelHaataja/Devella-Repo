using Devella.DataAccessLayer.Data;
using DevellaLib.Enums;
using DevellaLib.Models;
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
                FirstName = "Sten",
                Surname = "Andersson",
                EmailConfirmed = true,
                Created = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            var result = await userManager.CreateAsync(user, "DeveloperPass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Role.Developer.ToString());

                // Seed Competences for user
                var competence = new Competence
                {
                    UserId = user.Id,
                    Qualifications = new List<Qualification>
                    {
                        Qualification.DotNet,
                        Qualification.ReactJS,
                        Qualification.NodeJS
                    },
                    CompetenceAreas = new List<CompetenceArea>
                    {
                        CompetenceArea.SoftwareDevelopment,
                        CompetenceArea.FrontEnd,
                        CompetenceArea.BackEnd,
                    },
                    ProgrammingLanguages = new List<ProgrammingLanguage>
                    {
                        ProgrammingLanguage.CSharp,
                        ProgrammingLanguage.JavaScript,
                        ProgrammingLanguage.TypeScript,
                        ProgrammingLanguage.Python
                    },
                    CompetenceLevel = new List<CompetenceLevel>
                    {
                        CompetenceLevel.Intermediate,
                    }
                };

                // Created dev profile
                var developerProfile = new DeveloperUser
                {
                    UserId = user.Id,
                    Competence = competence,
                    Experience = 3,
                    School = "KTH",
                    WantedPosition = TypeOfPosition.FullTime,
                    Description = "Junior mjukvaruutvecklare med starka " +
                    "grundkunskaper inom objektorienterad programmering " +
                    "och webbutveckling. Har arbetat med mindre projekt " +
                    "i team där jag fått erfarenhet av versionshantering " +
                    "och agil metodik. Söker nu en möjlighet att tillämpa" +
                    " mina kunskaper och växa i en professionell miljö."
                };

                context.DeveloperUsers.Add(developerProfile);
                await context.SaveChangesAsync();
            }
        }

        // dev user 2
        var email2 = "Adam.Svensson@example.com";
        var user2 = await userManager.FindByEmailAsync(email2);

        if (user2 == null)
        {
            user2 = new User
            {
                UserName = email2,
                Email = email2,
                FirstName = "Adam",
                Surname = "Svensson",
                EmailConfirmed = true,
                Created = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            var result = await userManager.CreateAsync(user2, "DeveloperPass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user2, Role.Developer.ToString());

                // Seed Competences for user
                var competence = new Competence
                {
                    UserId = user2.Id,
                    Qualifications = new List<Qualification>
                    {
                        Qualification.VueJS,
                        Qualification.Ruby,
                    },
                    CompetenceAreas = new List<CompetenceArea>
                    {
                        CompetenceArea.SoftwareDevelopment,
                        CompetenceArea.FrontEnd
                    },
                    ProgrammingLanguages = new List<ProgrammingLanguage>
                    {
                        ProgrammingLanguage.CSharp,
                        ProgrammingLanguage.JavaScript
                    },
                    CompetenceLevel = new List<CompetenceLevel>
                    {
                        CompetenceLevel.Intermediate,
                    }
                };

                // Created dev profile
                var developerProfile = new DeveloperUser
                {
                    UserId = user2.Id,
                    Competence = competence,
                    Experience = 3,
                    School = "KYH",
                    WantedPosition = TypeOfPosition.LIA,
                    Description = "Driven och nyfiken mjukvaruutvecklare med " +
                    "grundläggande kunskaper i programmering och systemutveckling. " +
                    "Erfarenhet av projekt inom både skol- och fritidsmiljö, med " +
                    "fokus på problemlösning och teamwork. Strävar efter att lära " +
                    "mig nya teknologier och bidra med kreativitet i utvecklingsprocessen."
                };

                context.DeveloperUsers.Add(developerProfile);
                await context.SaveChangesAsync();
            }
        }

        // dev user 3
        var email3 = "sandra.eriksson@example.com";
        var user3 = await userManager.FindByEmailAsync(email3);

        if (user3 == null)
        {
            user3 = new User
            {
                UserName = email3,
                Email = email3,
                FirstName = "Sandra",
                Surname = "Eriksson",
                EmailConfirmed = true,
                Created = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            var result = await userManager.CreateAsync(user3, "DeveloperPass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user3, Role.Developer.ToString());

                // Seed Competences for user
                var competence = new Competence
                {
                    UserId = user3.Id,
                    Qualifications = new List<Qualification>
                    {
                        Qualification.ReactJS,
                        Qualification.NodeJS
                    },
                    CompetenceAreas = new List<CompetenceArea>
                    {
                        CompetenceArea.SoftwareDevelopment,
                        CompetenceArea.FrontEnd
                    },
                    ProgrammingLanguages = new List<ProgrammingLanguage>
                    {
                        ProgrammingLanguage.TypeScript,
                        ProgrammingLanguage.JavaScript
                    },
                    CompetenceLevel = new List<CompetenceLevel>
                    {
                        CompetenceLevel.Beginner,
                    }
                };

                // Created dev profile
                var developerProfile = new DeveloperUser
                {
                    UserId = user3.Id,
                    Competence = competence,
                    Experience = 3,
                    School = "KYH",
                    WantedPosition = TypeOfPosition.Internship,
                    Description = "Entusiastisk och noggrann mjukvaruutvecklare under " +
                    "utbildning med intresse för både frontend och backend. Erfarenhet " +
                    "av att skapa användarvänliga applikationer och lära mig moderna " +
                    "ramverk som React och Node.js. Ambition att bidra till kvalitativa " +
                    "och effektiva mjukvarulösningar i ett utvecklande team"
                };

                context.DeveloperUsers.Add(developerProfile);
                await context.SaveChangesAsync();
            }
        }
    }

    private static async Task SeedClientAsync(UserManager<User> userManager, ApplicationDbContext context)
    {
        var email = "company@example.com";
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                FirstName = "Sven",
                Surname = "Bengtsson",
                EmailConfirmed = true,
                Created = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            var result = await userManager.CreateAsync(user, "ClientPass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Role.Client.ToString());

                var companyProfile = new CompanyUser
                {
                    UserId = user.Id,
                    CompanyName = "Seeded Co",
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
                EmailConfirmed = true,
                Created = DateOnly.FromDateTime(DateTime.UtcNow)
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

