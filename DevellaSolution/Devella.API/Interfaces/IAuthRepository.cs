using Devella.DataAccessLayer.Models;

namespace Devella.API.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> ValidateUserAsync(LoginModel loginModel);
        Task<string?> GenerateJwtToken(User user);
    }
}
