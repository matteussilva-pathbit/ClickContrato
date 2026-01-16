namespace ClickContrato.Domain.Contracts;

public sealed class ContractTemplate
{
    public Guid Id { get; }
    public string Code { get; }
    public string Name { get; }
    public int Version { get; }
    public string Body { get; }
    public IReadOnlyList<ContractFieldDefinition> Fields { get; }

    public ContractTemplate(
        Guid id,
        string code,
        string name,
        int version,
        string body,
        IReadOnlyList<ContractFieldDefinition> fields)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id inválido.", nameof(id));
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code é obrigatório.", nameof(code));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name é obrigatório.", nameof(name));
        if (version <= 0) throw new ArgumentException("Version inválida.", nameof(version));
        if (string.IsNullOrWhiteSpace(body)) throw new ArgumentException("Body é obrigatório.", nameof(body));

        Id = id;
        Code = code.Trim().ToLowerInvariant();
        Name = name.Trim();
        Version = version;
        Body = body;
        Fields = fields ?? Array.Empty<ContractFieldDefinition>();
    }
}


