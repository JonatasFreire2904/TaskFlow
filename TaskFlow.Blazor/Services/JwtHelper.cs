using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

namespace TaskFlow.Blazor.Services
{
    public static class JwtHelper
    {
        public static Dictionary<string, string> ParseClaimsFromJwt(string jwt)
        {
            var parts = jwt.Split('.');
            if (parts.Length < 2) return new Dictionary<string, string>();

            var payload = parts[1];
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            var jsonBytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(jsonBytes);
            var doc = JsonDocument.Parse(json);

            var claims = new Dictionary<string, string>();
            foreach (var prop in doc.RootElement.EnumerateObject())
            {
                claims[prop.Name] = prop.Value.ToString();
            }
            return claims;
        }

        public static string? GetClaim(string jwt, string claimType)
        {
            var d = ParseClaimsFromJwt(jwt);
            return d.TryGetValue(claimType, out var v) ? v : null;
        }
    }
}
