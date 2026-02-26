using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace AdventureWorks.Web.Services;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _js;
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;
    private const string TokenKey = "authToken";

    public ApiAuthenticationStateProvider(IJSRuntime js, HttpClient http, NavigationManager navigation)
    {
        _js = js;
        _http = http;
        _navigation = navigation;
    }

    private static bool IsTokenExpired(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length < 2) return false;

            var payload = parts[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            using var doc = JsonDocument.Parse(jsonBytes);

            if (doc.RootElement.TryGetProperty("exp", out var expEl))
            {
                long seconds;
                if (expEl.ValueKind == JsonValueKind.Number && expEl.TryGetInt64(out seconds))
                {
                    // ok
                }
                else if (expEl.ValueKind == JsonValueKind.String && long.TryParse(expEl.GetString(), out seconds))
                {
                    // ok
                }
                else
                {
                    return false;
                }

                var expiry = DateTimeOffset.FromUnixTimeSeconds(seconds);
                return expiry <= DateTimeOffset.UtcNow;
            }
        }
        catch
        {
            // if parsing fails, assume not expired to avoid logging out unexpectedly
        }

        return false;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var storedToken = await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);
            var token = NormalizeToken(storedToken);
            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // If token is expired, clear it and return anonymous principal
            if (IsTokenExpired(token))
            {
                await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
                _http.DefaultRequestHeaders.Authorization = null;
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // set default header
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        token = NormalizeToken(token) ?? string.Empty;
        if (string.IsNullOrWhiteSpace(token))
        {
            await MarkUserAsLoggedOut();
            return;
        }

        // If incoming token is already expired, do not persist it
        if (IsTokenExpired(token))
        {
            await MarkUserAsLoggedOut();
            return;
        }

        await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        // Navigate to main page after successful authentication
        try
        {
            _navigation.NavigateTo("/home");
        }
        catch
        {
            // ignore navigation errors in provider
        }

        // end MarkUserAsAuthenticated
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        _http.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes);

        if (keyValuePairs == null)
            return Array.Empty<Claim>();

        var claims = new List<Claim>();
        foreach (var kvp in keyValuePairs)
        {
            if (kvp.Value.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in kvp.Value.EnumerateArray())
                {
                    claims.Add(new Claim(kvp.Key, item.ToString()));
                }
            }
            else
            {
                claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
            }
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }

    private static string? NormalizeToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        token = token.Trim();

        // Some APIs return the token as a quoted string or with a Bearer prefix.
        token = token.Trim('"');

        const string bearerPrefix = "Bearer ";
        if (token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            token = token[bearerPrefix.Length..].Trim();
        }

        return string.IsNullOrWhiteSpace(token) ? null : token;
    }
}
