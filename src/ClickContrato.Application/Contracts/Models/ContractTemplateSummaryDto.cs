namespace ClickContrato.Application.Contracts.Models;

public sealed record ContractTemplateSummaryDto(
    Guid Id,
    string Code,
    string Name,
    int Version,
    IReadOnlyList<ContractTemplateFieldDto> Fields
);

public sealed record ContractTemplateFieldDto(
    string Key,
    string Label,
    string Type,
    bool Required,
    string? Placeholder,
    int? MaxLength
);


