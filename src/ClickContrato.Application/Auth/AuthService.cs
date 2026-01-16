namespace ClickContrato.Application.Auth;

public sealed class AuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwt;

    public AuthService(IUserRepository users, IPasswordHasher passwordHasher, IJwtTokenGenerator jwt)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var email = (request.Email ?? string.Empty).Trim().ToLowerInvariant();
        var name = (request.Name ?? string.Empty).Trim();
        var password = request.Password ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return Result<AuthResponse>.Failure("invalid_email", "Email inválido.");

        if (name.Length < 2)
            return Result<AuthResponse>.Failure("invalid_name", "Nome inválido.");

        if (password.Length < 8)
            return Result<AuthResponse>.Failure("weak_password", "Senha deve ter pelo menos 8 caracteres.");

        var existing = await _users.FindByEmailAsync(email, ct);
        if (existing is not null)
            return Result<AuthResponse>.Failure("email_taken", "Já existe usuário com esse email.");

        var hash = _passwordHasher.Hash(password);
        var user = new User(Guid.NewGuid(), email, name, hash, UserRole.User, DateTime.UtcNow);
        await _users.AddAsync(user, ct);

        var (token, expiresAtUtc) = _jwt.Generate(user);
        return Result<AuthResponse>.Success(new AuthResponse(token, expiresAtUtc));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var email = (request.Email ?? string.Empty).Trim().ToLowerInvariant();
        var password = request.Password ?? string.Empty;

        var user = await _users.FindByEmailAsync(email, ct);
        if (user is null)
            return Result<AuthResponse>.Failure("invalid_credentials", "Credenciais inválidas.");

        if (!_passwordHasher.Verify(password, user.PasswordHash))
            return Result<AuthResponse>.Failure("invalid_credentials", "Credenciais inválidas.");

        var (token, expiresAtUtc) = _jwt.Generate(user);
        return Result<AuthResponse>.Success(new AuthResponse(token, expiresAtUtc));
    }
}


