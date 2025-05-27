using Devella.DataAccessLayer.DTOs.UserAccess;
using DevellaLib.Services.Paging;

namespace Devella.Interfaces;

public interface IDeveloperProvider
{
    Task<DeveloperProfileDTO> GetProfileAsync();
    Task<IEnumerable<DeveloperProfileDTO>> GetAllProfilesAsync();
    Task<PagedResult<DeveloperProfileDTO>> GetAllPagedProfilesAsync(int currentPage, int pageSize, string? searchTerm, string? sortOption);
    Task<bool> UpdateProfileAsync(UpdateDevProfileDTO dto);
}
