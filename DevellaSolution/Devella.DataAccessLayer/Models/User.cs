
using Microsoft.AspNetCore.Identity;

namespace Devella.DataAccessLayer.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }  
    public string Surname { get; set; }
}
