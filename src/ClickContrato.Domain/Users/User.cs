namespace ClickContrato.Domain.Users;

public sealed class User
{
    public Guid Id { get; }
    public string Email { get; }
    public string Name { get; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAtUtc { get; }

    public User(Guid id, string email, string name, string passwordHash, UserRole role, DateTime createdAtUtc)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id inválido.", nameof(id));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email é obrigatório.", nameof(email));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nome é obrigatório.", nameof(name));
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("PasswordHash é obrigatório.", nameof(passwordHash));

        Id = id;
        Email = email.Trim().ToLowerInvariant();
        Name = name.Trim();
        PasswordHash = passwordHash;
        Role = role;
        CreatedAtUtc = createdAtUtc;
    }

    public void PromoteToAdmin() => Role = UserRole.Admin;
}


