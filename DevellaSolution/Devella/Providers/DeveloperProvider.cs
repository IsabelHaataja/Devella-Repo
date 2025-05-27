using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.Interfaces;
using DevellaLib.Services.Paging;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Devella.Providers;

public class DeveloperProvider : IDeveloperProvider
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _localStorage;

    public DeveloperProvider(HttpClient httpClient, ProtectedLocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<DeveloperProfileDTO> GetProfileAsync()
    {
        await SetAuthorizationHeaderAsync();

        try
        {
            var response = await _httpClient.GetAsync("api/developer/profile");
            response.EnsureSuccessStatusCode();

            var profile = await response.Content.ReadFromJsonAsync<DeveloperProfileDTO>();
            if (profile == null)
                throw new Exception("Failed to deserialize developer profile.");

            return profile;
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP error fetching developer profile: {httpEx.Message}");
            throw;
        }
        catch (NotSupportedException ex) // Content type is not valid
        {
            Console.WriteLine($"Unsupported content type: {ex.Message}");
            throw;
        }
        catch (JsonException ex) // Invalid JSON
        {
            Console.WriteLine($"JSON parse error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error fetching developer profile: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<DeveloperProfileDTO>> GetAllProfilesAsync()
    {
        await SetAuthorizationHeaderAsync();
        try
        {
            var response = await _httpClient.GetAsync("api/developer/profiles");
            response.EnsureSuccessStatusCode();
            var profiles = await response.Content.ReadFromJsonAsync<IEnumerable<DeveloperProfileDTO>>();
            if (profiles == null)
                throw new Exception("Failed to deserialize developer profiles.");

            Console.WriteLine("Returning all profiles successfully");
            return profiles;
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP error fetching developer profiles: {httpEx.Message}");
            throw;
        }
        catch (NotSupportedException ex) // Content type is not valid
        {
            Console.WriteLine($"Unsupported content type: {ex.Message}");
            throw;
        }
        catch (JsonException ex) // Invalid JSON
        {
            Console.WriteLine($"JSON parse error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error fetching developer profiles: {ex.Message}");
            throw;
        }
    }

    public async Task<PagedResult<DeveloperProfileDTO>> GetAllPagedProfilesAsync(int currentPage, int pageSize, string? searchTerm, string? sortOption)
    {
        await SetAuthorizationHeaderAsync();

        try
        {
            var response = await _httpClient.GetAsync($"api/developer/paged?pageNumber={currentPage}&pageSize={pageSize}&searchTerm={searchTerm}&sortOption={sortOption}");
            response.EnsureSuccessStatusCode();

            var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<DeveloperProfileDTO>>();

            return pagedResult ?? new PagedResult<DeveloperProfileDTO>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retreive paged profiles: {ex.Message}");
            return new PagedResult<DeveloperProfileDTO>();
        }
    }

    public async Task<bool> UpdateProfileAsync(UpdateDevProfileDTO dto)
    {
        await SetAuthorizationHeaderAsync();

        try
        {
            var response = await _httpClient.PatchAsJsonAsync("api/developer/update", dto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to update developer profile: {errorContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating developer profile: {ex.Message}");
            throw;
        }
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
