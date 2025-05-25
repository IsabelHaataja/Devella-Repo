using Devella.DataAccessLayer.Enums;

namespace Devella.DataAccessLayer.DTOs;

public class CompetenceDTO
{
    public List<Qualification>? Qualifications { get; set; } = new();
    public List<CompetenceArea>? CompetenceAreas { get; set; } = new();
    public List<ProgrammingLanguage>? ProgrammingLanguages { get; set; } = new();
    public List<CompetenceLevel>? CompetenceLevel { get; set; } = new();
}
