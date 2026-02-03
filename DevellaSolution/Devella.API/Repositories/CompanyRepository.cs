using Devella.API.Interfaces;
using Devella.DataAccessLayer.Data;
using Devella.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Devella.API.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SaveDeveloperToListAsync(string companyUserId, int developerId)
    {
        var companyUser = await _context.CompanyUsers
            .FirstOrDefaultAsync(c => c.UserId == companyUserId);

        if (companyUser == null)
            throw new Exception($"Company user with ID '{companyUserId}' not found");

        var developerExists = await _context.DeveloperUsers.AnyAsync(d => d.Id == developerId);
        if (!developerExists)
            throw new Exception($"Developer with ID '{developerId}' not found");

        // Avoids duplicate entry
        if (!companyUser.DeveloperIds.Contains(developerId))
        {
            companyUser.DeveloperIds.Add(developerId);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<DeveloperUser>> GetSavedDevelopersAsync(string userId)
    {
        var companyUser = await _context.CompanyUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (companyUser == null || companyUser.DeveloperIds.Count == 0)
            return new List<DeveloperUser>();

        var developers = await _context.DeveloperUsers
            .AsNoTracking()
            .Where(d => companyUser.DeveloperIds.Contains(d.Id))
            .Include(d => d.User)
            .Include(d => d.Competence)
            .ToListAsync();

        return developers;
    }
}
