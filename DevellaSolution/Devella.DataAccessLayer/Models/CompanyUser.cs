
namespace Devella.DataAccessLayer.Models;

public class CompanyUser
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public string? CompanyName { get; set; }

    public List<DeveloperUser> SavedDevelopers { get; set; } = new();
}
