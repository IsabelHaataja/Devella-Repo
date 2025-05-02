using Devella.API.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Devella.API.Services
{
    public class AuthService : IAuthService
    {
        public List<string> GetUserRolesFromToken(string token)
        {
            var roles = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(token)) throw new ArgumentException("Token cannot be null or empty.");

                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(token);

                // Check if roles are present in the token
                var roleClaims = jwtToken.Claims
                    .Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                    .ToList();

                if (roleClaims.Any())
                {
                    roles = roleClaims.Select(c => c.Value).ToList();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decoding token: {ex.Message}");
            }

            return roles;
        }
    }
}
