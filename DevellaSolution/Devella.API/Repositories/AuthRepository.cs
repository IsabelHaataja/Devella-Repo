using Devella.API.Interfaces;
using Devella.DataAccessLayer.Data;
using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.DataAccessLayer.Enums;
using Devella.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Devella.API.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public AuthRepository(UserManager<User> userManager, IConfiguration configuration, ApplicationDbContext context)
    {
        _userManager = userManager;
        _configuration = configuration;
        _context = context;
    }

    public async Task<User?> ValidateUserAsync(LoginModel loginModel)
    {
        var user = await _userManager.FindByEmailAsync(loginModel.Email);
        if (user == null || user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow)
            return null;

        var result = await _userManager.CheckPasswordAsync(user, loginModel.Password);
        return result ? user : null;
    }

    public async Task<string?> GenerateJwtToken(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        claims.AddRange(userClaims);
        claims.AddRange(roleClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<IdentityResult> CreateUserAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task AddUserToRoleAsync(User user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task CreateDeveloperProfileAsync(string userId)
    {
        var developer = new DeveloperUser
        {
            UserId = userId,
            Competence = null,
            Experience = 0,
            Description = null,
            School = null,
            WantedPosition = TypeOfPosition.OpenToAll
        };

        _context.DeveloperUsers.Add(developer);
        await _context.SaveChangesAsync();
    }

    // TODO: finish method
    public async Task CreateClientProfileAsync(string userId)
    {
        var client = new CompanyUser
        {
            UserId = userId,

        };

        _context.CompanyUsers.Add(client);
        await _context.SaveChangesAsync();
    }

    public async Task CreateAdminProfileAsync(string userId)
    {
        var admin = new AdminUser
        {
            //UserId = userId,

        };

        _context.AdminUsers.Add(admin);
        await _context.SaveChangesAsync();
    }
}
