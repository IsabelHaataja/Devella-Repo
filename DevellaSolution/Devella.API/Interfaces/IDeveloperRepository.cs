using DevellaLib.DTOs.UserAccess;
using DevellaLib.Models;

namespace Devella.API.Interfaces;

public interface IDeveloperRepository
{
    Task<DeveloperUser?> GetDeveloperProfileAsync(string userId);
    Task<IEnumerable<DeveloperUser?>> GetDeveloperProfilesAsync();
    IQueryable<DeveloperUser> GetAllQueryable();
    Task<DeveloperUser?> UpdateDeveloperProfileAsync(string userId, UpdateDevProfileDTO dto);
}
