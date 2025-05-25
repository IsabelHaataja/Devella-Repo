using Devella.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Devella.API.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> ValidateUserAsync(LoginModel loginModel);
        Task<string?> GenerateJwtToken(User user);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task AddUserToRoleAsync(User user, string role);
        Task CreateDeveloperProfileAsync(string userId);
        Task CreateClientProfileAsync(string userId);
        Task CreateAdminProfileAsync(string userId);
    }
}
