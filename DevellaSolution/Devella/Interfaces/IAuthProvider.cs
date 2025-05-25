using Devella.DataAccessLayer.DTOs.UserAccess;

namespace Devella.Interfaces;

public interface IAuthProvider
{
    Task SetAuthorizationHeaderAsync();
    Task<T1> PostAsync<T1, T2>(string url, T2 content);
    Task<bool> RegisterUserAsync(RegisterDTO model);
}
