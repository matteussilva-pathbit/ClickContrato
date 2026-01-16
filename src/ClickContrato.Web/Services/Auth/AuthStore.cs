using ClickContrato.Web.Models;
using ClickContrato.Web.Services.Api;
using ClickContrato.Web.Services.Storage;

namespace ClickContrato.Web.Services.Auth;

public sealed class AuthStore
{
    private const string TokenKey = "cc.auth.token";
    private const string ExpKey = "cc.auth.expiresAtUtc";

    private readonly IKeyValueStorage _storage;
    private readonly ClickContratoApiClient _api;

    public AuthStore(IKeyValueStorage storage, ClickContratoApiClient api)
    {
        _storage = storage;
        _api = api;
    }

    public string? AccessToken { get; private set; }
    public DateTime? ExpiresAtUtc { get; private set; }

    public bool IsAuthenticated =>
        !string.IsNullOrWhiteSpace(AccessToken) &&
        ExpiresAtUtc is not null &&
        ExpiresAtUtc.Value > DateTime.UtcNow.AddMinutes(1);

    public async Task InitializeAsync()
    {
        if (AccessToken is not null) return;

        var token = await _storage.GetAsync(TokenKey);
        var exp = await _storage.GetAsync(ExpKey);

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(exp))
            return;

        if (!DateTime.TryParse(exp, out var expiresAtUtc))
            return;

        AccessToken = token;
        ExpiresAtUtc = DateTime.SpecifyKind(expiresAtUtc, DateTimeKind.Utc);
        _api.SetBearerToken(AccessToken);
    }

    public async Task SetSessionAsync(AuthResponse auth)
    {
        AccessToken = auth.AccessToken;
        ExpiresAtUtc = auth.ExpiresAtUtc;

        _api.SetBearerToken(AccessToken);

        await _storage.SetAsync(TokenKey, auth.AccessToken);
        await _storage.SetAsync(ExpKey, auth.ExpiresAtUtc.ToString("O"));
    }

    public async Task LogoutAsync()
    {
        AccessToken = null;
        ExpiresAtUtc = null;
        _api.SetBearerToken(null);

        await _storage.RemoveAsync(TokenKey);
        await _storage.RemoveAsync(ExpKey);
    }
}


