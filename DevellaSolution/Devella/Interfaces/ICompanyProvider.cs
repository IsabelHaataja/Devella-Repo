using DevellaLib.DTOs.UserAccess;

namespace Devella.Interfaces
{
    public interface ICompanyProvider
    {
        Task<bool> SaveDeveloperToListAsync(int developerId);
        Task<List<DeveloperProfileDTO>> GetSavedDevelopersAsync();
    }
}
