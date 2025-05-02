using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Cryptography;

namespace Devella.API.Services
{
    public static class StorageHelper
    {
        public static async Task<string> TryGetProtectedItem<T>(ProtectedLocalStorage storage, string key, bool isProduction)
        {
            try
            {
                var result = (await storage.GetAsync<string>(key)).Value;
                if (result != null)
                {
                    Console.WriteLine($"[StorageHelpers] Successfully retrieved token for key '{key}'.");
                    return result;
                }
                else
                {
                    Console.WriteLine($"[StorageHelpers] Token not found for key '{key}'. Returning default.");
                    return default;
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"[StorageHelpers] CryptographicException for key '{key}': {ex.Message}");

                if (isProduction)
                {
                    await storage.DeleteAsync(key); // Remove corrupted data
                }
                else
                {
                    // In development
                    Console.WriteLine("[StorageHelpers] In development mode, not deleting token.");
                }

                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StorageHelpers] Unexpected error for key '{key}': {ex.Message}");
                return default;
            }
        }
    }
}
