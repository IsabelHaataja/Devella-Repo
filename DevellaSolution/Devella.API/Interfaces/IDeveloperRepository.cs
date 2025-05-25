using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.DataAccessLayer.Models;

namespace Devella.API.Interfaces;

public interface IDeveloperRepository
{
    Task<DeveloperUser?> GetDeveloperProfileAsync(string userId);
    Task<DeveloperUser?> UpdateDeveloperProfileAsync(string userId, UpdateDevProfileDTO dto);
}
