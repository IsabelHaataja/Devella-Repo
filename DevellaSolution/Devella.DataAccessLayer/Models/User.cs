
using Devella.DataAccessLayer.Enums;
using Microsoft.AspNetCore.Identity;

namespace Devella.DataAccessLayer.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }  
    public string Surname { get; set; }
    public DeveloperUser? DeveloperProfile { get; set; }
    public CompanyUser? CompanyProfile { get; set; }
    public AdminUser? AdminProfile { get; set; }
}
