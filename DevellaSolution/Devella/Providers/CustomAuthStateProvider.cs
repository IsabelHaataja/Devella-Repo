using Devella.API.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Devella.Providers;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
    private readonly ProtectedLocalStorage _localStorage;
    private readonly IHostEnvironment _environment;

    public CustomAuthStateProvider(HttpClient httpClient, ProtectedLocalStorage localStorage, IHostEnvironment environment)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _environment = environment;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Determine if the app is running in production
            bool isProduction = _environment.IsProduction();

            var token = await StorageHelper.TryGetProtectedItem<string>(_localStorage, "authToken", isProduction);

            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : GetClaimsIdentity(token);
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not get authentication state: {ex.Message}");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    private ClaimsIdentity GetClaimsIdentity(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var claims = jwtToken.Claims;

        var fullNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

        return new ClaimsIdentity(claims, "jwt");
    }

    public async Task RefreshUserAuthenticationState()
    {
        try
        {
            var token = (await _localStorage.GetAsync<string>("authToken")).Value;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"Authorization Header Set token: {token}");

                var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                _currentUser = new ClaimsPrincipal(identity);
            }
            else
            {
                _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            }

            // Trigger re-render after authentication state is set
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error accessing localStorage.");
        }

    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = Convert.FromBase64String(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    public async Task MarkUserAsLoggedIn(string token)
    {
        await _localStorage.SetAsync("authToken", token);
        var identity = GetClaimsIdentity(token);
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.DeleteAsync("authToken");
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
}
