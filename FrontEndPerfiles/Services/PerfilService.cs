using System.Net.Http.Json;
using System.Net.Http.Headers;
using FrontEndPerfiles.Models;

namespace FrontEndPerfiles.Services
{
    public class PerfilService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5159/api"; // Puerto del backend

        public PerfilService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<Perfil>?> GetPerfilesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Perfil>>($"{_baseUrl}/Perfiles");
            }
            catch
            {
                return null;
            }
        }

        public async Task<Perfil?> GetPerfilAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Perfil>($"{_baseUrl}/Perfiles/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdatePerfilAsync(int id, Perfil perfil)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/Perfiles/{id}", perfil);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
