
using Devella.DataAccessLayer.Enums;

namespace Devella.DataAccessLayer.Models
{
    public class Competence
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public List<Qualification> Qualifications { get; set; } = new();
        public List<CompetenceArea> CompetenceAreas { get; set; } = new();
        public List<ProgrammingLanguage> ProgrammingLanguages { get; set; } = new();
        public List<CompetenceLevel> CompetenceLevel { get; set; } = new();
    }
}
