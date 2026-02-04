namespace DevellaLib.Models;

public class CompanyUser
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public string? CompanyName { get; set; }

    public List<int> DeveloperIds { get; set; } = new();
}