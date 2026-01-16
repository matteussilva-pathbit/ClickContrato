namespace ClickContrato.Application.Auth.Models;

public sealed record AuthResponse(string AccessToken, DateTime ExpiresAtUtc);


