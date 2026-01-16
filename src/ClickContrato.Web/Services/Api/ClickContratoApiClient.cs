using System.Net.Http.Headers;
using System.Net.Http.Json;
using ClickContrato.Web.Models;

namespace ClickContrato.Web.Services.Api;

public sealed class ClickContratoApiClient
{
    private readonly HttpClient _http;

    public ClickContratoApiClient(HttpClient http) => _http = http;

    public void SetBearerToken(string? token)
    {
        _http.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("/auth/register", request, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: ct))!;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("/auth/login", request, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: ct))!;
    }

    public async Task<List<ContractTemplateSummaryDto>> ListTemplatesAsync(CancellationToken ct = default)
    {
        var resp = await _http.GetAsync("/contract-templates", ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<List<ContractTemplateSummaryDto>>(cancellationToken: ct))!;
    }

    public async Task<ContractDraftDto> CreateDraftAsync(CreateDraftRequest request, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("/contracts/drafts", request, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<ContractDraftDto>(cancellationToken: ct))!;
    }

    public async Task<ContractDraftDto> UpdateDraftFieldsAsync(Guid draftId, UpdateDraftFieldsRequest request, CancellationToken ct = default)
    {
        var resp = await _http.PutAsJsonAsync($"/contracts/drafts/{draftId}/fields", request, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<ContractDraftDto>(cancellationToken: ct))!;
    }

    public async Task<string> GetDraftPreviewAsync(Guid draftId, CancellationToken ct = default)
    {
        var resp = await _http.GetAsync($"/contracts/drafts/{draftId}/preview", ct);
        resp.EnsureSuccessStatusCode();
        var payload = await resp.Content.ReadFromJsonAsync<PreviewResponse>(cancellationToken: ct);
        return payload?.Preview ?? string.Empty;
    }
}


