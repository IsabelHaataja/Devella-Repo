using Microsoft.AspNetCore.Identity;
namespace DevellaLib.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public DeveloperUser? DeveloperProfile { get; set; }
    public CompanyUser? CompanyProfile { get; set; }
    public AdminUser? AdminProfile { get; set; }
    public DateOnly Created { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
}