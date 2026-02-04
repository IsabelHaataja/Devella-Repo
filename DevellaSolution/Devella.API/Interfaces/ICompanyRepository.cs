using DevellaLib.Models;

namespace Devella.API.Interfaces;

public interface ICompanyRepository
{
    Task SaveDeveloperToListAsync(string companyUserId, int developerId);
    Task<List<DeveloperUser>> GetSavedDevelopersAsync(string userId);
}
