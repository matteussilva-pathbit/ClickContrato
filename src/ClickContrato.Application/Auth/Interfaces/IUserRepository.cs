namespace ClickContrato.Application.Auth.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
}


