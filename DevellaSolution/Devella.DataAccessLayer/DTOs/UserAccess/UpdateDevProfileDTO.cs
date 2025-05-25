
using Devella.DataAccessLayer.Enums;

namespace Devella.DataAccessLayer.DTOs.UserAccess;

public class UpdateDevProfileDTO
{
    public string? Description { get; set; }
    public string? School { get; set; }
    public int? Experience { get; set; }
    public TypeOfPosition? WantedPosition { get; set; }
    public List<Qualification> Qualifications { get; set; } = new();
    public List<CompetenceArea> CompetenceAreas { get; set; } = new();
    public List<ProgrammingLanguage> ProgrammingLanguages { get; set; } = new();
    public List<CompetenceLevel> CompetenceLevel { get; set; } = new();
}
