using System.Net.Http.Json;
using FrontEndPerfiles.Models;

namespace FrontEndPerfiles.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5159/api"; // Puerto del backend

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Auth/login", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Perfil?> RegisterAsync(Perfil perfil)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Auth/register", perfil);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Perfil>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
