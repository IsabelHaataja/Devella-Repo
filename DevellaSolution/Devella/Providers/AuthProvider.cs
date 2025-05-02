using Devella.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace Devella.Providers
{
    public class AuthProvider : IAuthProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;

        public AuthProvider(HttpClient httpClient, ProtectedLocalStorage localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<T1> PostAsync<T1, T2>(string url, T2 content)
        {
            await SetAuthorizationHeaderAsync();
            Console.WriteLine($"[AuthProvider] Sending POST request to: {url}");

            var response = await _httpClient.PostAsJsonAsync(url, content);
            if (response != null && response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T1>(await response.Content.ReadAsStringAsync());
            }
            Console.WriteLine($"[AuthProvider] Response Status: {response.StatusCode}");
            return default;
        }

        public async Task SetAuthorizationHeaderAsync()
        {
            var token = (await _localStorage.GetAsync<string>("authToken")).Value;
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
