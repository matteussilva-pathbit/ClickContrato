namespace ClickContrato.Infrastructure.Auth;

public sealed class InMemoryUserRepository : IUserRepository
{
    private static readonly ConcurrentDictionary<string, User> UsersByEmail = new(StringComparer.OrdinalIgnoreCase);

    public Task<User?> FindByEmailAsync(string email, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email)) return Task.FromResult<User?>(null);
        UsersByEmail.TryGetValue(email.Trim().ToLowerInvariant(), out var user);
        return Task.FromResult(user);
    }

    public Task AddAsync(User user, CancellationToken ct)
    {
        UsersByEmail[user.Email] = user;
        return Task.CompletedTask;
    }
}


