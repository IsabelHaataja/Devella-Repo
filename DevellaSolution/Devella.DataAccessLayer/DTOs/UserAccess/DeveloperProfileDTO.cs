
using Devella.DataAccessLayer.Enums;
using Devella.DataAccessLayer.Models;

namespace Devella.DataAccessLayer.DTOs.UserAccess;

public class DeveloperProfileDTO
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Description { get; set; }
    public string? School { get; set; }
    public int Experience { get; set; }
    public CompetenceDTO? Competence { get; set; }
    public TypeOfPosition? WantedPosition { get; set; }
}
