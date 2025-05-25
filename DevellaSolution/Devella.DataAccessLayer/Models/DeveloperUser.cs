using Devella.DataAccessLayer.Enums;

namespace Devella.DataAccessLayer.Models;

public class DeveloperUser
{
    public int Id { get; set; }
    public User User { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public Competence? Competence { get; set; }
    public int Experience { get; set; }
    public string? Description { get; set; }
    public string? School { get; set; }
    public TypeOfPosition WantedPosition { get; set; }
}
