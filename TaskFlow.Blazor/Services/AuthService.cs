using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TaskFlow.Blazor.Models;

namespace TaskFlow.Blazor.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly TokenProvider _tokenProvider;

        // Recebe um HttpClient (pode ser o padrão ou o com auth)
        public AuthService(HttpClient http, TokenProvider tokenProvider)
        {
            _http = http;
            _tokenProvider = tokenProvider;
        }

        public async Task<bool> LoginAsync(LoginRequest req)
        {
            var resp = await _http.PostAsJsonAsync("api/auth/login", req);
            if (!resp.IsSuccessStatusCode) return false;

            var json = await resp.Content.ReadFromJsonAsync<JsonElement>();
            if (json.ValueKind == JsonValueKind.Object && json.TryGetProperty("token", out var tokenProp))
            {
                var token = tokenProp.GetString();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    await _tokenProvider.SetTokenAsync(token);
                    return true;
                }
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            await _tokenProvider.RemoveTokenAsync();
        }
    }
}
