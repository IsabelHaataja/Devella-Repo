using Devella.DataAccessLayer.DTOs.UserAccess;

namespace Devella.Interfaces;

public interface IDeveloperProvider
{
    Task<DeveloperProfileDTO> GetProfileAsync();
    Task<bool> UpdateProfileAsync(UpdateDevProfileDTO dto);
}
