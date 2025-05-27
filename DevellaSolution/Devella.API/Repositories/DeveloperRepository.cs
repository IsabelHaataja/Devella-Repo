using Devella.API.Interfaces;
using Devella.DataAccessLayer.Data;
using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.DataAccessLayer.Mappers.Developer;
using Devella.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Devella.API.Repositories;

public class DeveloperRepository : IDeveloperRepository
{
    private readonly ApplicationDbContext _context;

    public DeveloperRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeveloperUser?> GetDeveloperProfileAsync(string userId)
    {
        try
        {
            return await _context.DeveloperUsers
                .Include(d => d.User)                
                .Include(d => d.Competence)
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retreive developer profile info: {ex.Message}");
            return null;
        }
    }

    public async Task<IEnumerable<DeveloperUser?>> GetDeveloperProfilesAsync()
    {
        try
        {
            return await _context.DeveloperUsers
                .Include(d => d.User)
                .Include(d => d.Competence)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retreive developer profile info: {ex.Message}");
            return null;
        }
    }

    public IQueryable<DeveloperUser> GetAllQueryable()
    {
        return _context.DeveloperUsers
            .Include(d => d.User)
            .Include(d => d.Competence);
    }

    public async Task<DeveloperUser?> UpdateDeveloperProfileAsync(string userId, UpdateDevProfileDTO dto)
    {
        try
        {
            var developer = await GetDeveloperProfileAsync(userId);
            if (developer == null)
                return null;

            DeveloperMapper.UpdateFromDto(developer, dto);
            await _context.SaveChangesAsync();

            return developer;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retreive developer profile info: {ex.Message}");
            return null;
        }
    }
}
