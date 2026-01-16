namespace ClickContrato.Domain.Contracts;

public sealed record ContractFieldDefinition(
    string Key,
    string Label,
    FieldType Type,
    bool Required,
    string? Placeholder = null,
    int? MaxLength = null
);


