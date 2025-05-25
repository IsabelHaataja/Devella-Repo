
using Devella.DataAccessLayer.DTOs;
using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.DataAccessLayer.Models;

namespace Devella.DataAccessLayer.Mappers.Developer;

public static class DeveloperMapper
{
    public static DeveloperProfileDTO ToDto(DeveloperUser dev)
    {
        return new DeveloperProfileDTO
        {
            Id = dev.Id,
            UserId = dev.UserId,
            FirstName = dev.User.FirstName,
            Surname = dev.User.Surname,
            Description = dev.Description,
            School = dev.School,
            Experience = dev.Experience,
            WantedPosition = dev.WantedPosition,
            Competence = dev.Competence == null ? null : new CompetenceDTO
            {
                Qualifications = dev.Competence.Qualifications.ToList(),
                CompetenceAreas = dev.Competence.CompetenceAreas.ToList(),
                ProgrammingLanguages = dev.Competence.ProgrammingLanguages.ToList(),
                CompetenceLevel = dev.Competence.CompetenceLevel.ToList()
            }
        };
    }

    public static void UpdateFromDto(DeveloperUser developer, UpdateDevProfileDTO dto)
    {
        if (dto.Description != null) developer.Description = dto.Description;
        if (dto.School != null) developer.School = dto.School;
        if (dto.Experience.HasValue) developer.Experience = dto.Experience.Value;
        if (dto.WantedPosition.HasValue) developer.WantedPosition = dto.WantedPosition.Value;

        if (developer.Competence == null)
            developer.Competence = new Competence { UserId = developer.UserId };

        // Assign lists directly (they are never null)
        developer.Competence.Qualifications = dto.Qualifications;
        developer.Competence.CompetenceAreas = dto.CompetenceAreas;
        developer.Competence.ProgrammingLanguages = dto.ProgrammingLanguages;
    }
    //public static DeveloperUser UpdateFromDto(DeveloperUser developer, UpdateDevProfileDTO dto)
    //{
    //    if (dto.Description != null) developer.Description = dto.Description;
    //    if (dto.School != null) developer.School = dto.School;
    //    if (dto.Experience.HasValue) developer.Experience = dto.Experience.Value;
    //    if (dto.WantedPosition.HasValue) developer.WantedPosition = dto.WantedPosition.Value;

    //    if (developer.Competence == null)
    //        developer.Competence = new Competence { UserId = developer.UserId };

    //    // Assign lists directly (they are never null)
    //    developer.Competence.Qualifications = dto.Qualifications;
    //    developer.Competence.CompetenceAreas = dto.CompetenceAreas;
    //    developer.Competence.ProgrammingLanguages = dto.ProgrammingLanguages;
    //    developer.Competence.CompetenceLevel = dto.CompetenceLevel;
    //}
}
