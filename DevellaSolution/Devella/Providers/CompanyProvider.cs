using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Devella.Providers;

public class CompanyProvider : ICompanyProvider
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _localStorage;

    public CompanyProvider(HttpClient httpClient, ProtectedLocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<bool> SaveDeveloperToListAsync(int developerId)
    {
        await SetAuthorizationHeaderAsync();

        try
        {
            var response = await _httpClient.PostAsync($"api/company/save-developer/{developerId}", null);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving developer: {ex.Message}");
            return false;
        }
    }

    public async Task<List<DeveloperProfileDTO>> GetSavedDevelopersAsync()
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.GetAsync("api/company/saved-developers");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<DeveloperProfileDTO>>();
        return result ?? new List<DeveloperProfileDTO>();
    }


    private async Task SetAuthorizationHeaderAsync()
    {
        var token = (await _localStorage.GetAsync<string>("authToken")).Value;
        if (token != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
