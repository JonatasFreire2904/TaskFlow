using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace TaskFlow.Blazor.Services
{
    public class TokenProvider
    {
        private readonly IJSRuntime _js;
        private const string TOKEN_KEY = "jwt";
        private const string USERID_KEY = "userId";

        public TokenProvider(IJSRuntime js)
        {
            _js = js;
        }

        public ValueTask SetTokenAsync(string token) => _js.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, token);
        public ValueTask RemoveTokenAsync() => _js.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
        public ValueTask<string?> GetTokenAsync() => _js.InvokeAsync<string?>("localStorage.getItem", TOKEN_KEY);

        // helper para gravar qualquer par chave/valor (usado para salvar userId)
        public ValueTask SetItemAsync(string key, string value) => _js.InvokeVoidAsync("localStorage.setItem", key, value);
        public ValueTask<string?> GetItemAsync(string key) => _js.InvokeAsync<string?>("localStorage.getItem", key);
    }
}
