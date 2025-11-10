using System.Net.Http.Json;
using System.Net.Http.Headers;
using FrontEndPerfiles.Models;

namespace FrontEndPerfiles.Services
{
    public class ComentarioService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5159/api"; // Puerto del backend

        public ComentarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<Comentario>?> GetComentariosByPerfilAsync(int perfilId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Comentario>>($"{_baseUrl}/Comentarios/ByPerfil/{perfilId}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<Comentario?> CreateComentarioAsync(Comentario comentario)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Comentarios", comentario);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Comentario>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteComentarioAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/Comentarios/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
