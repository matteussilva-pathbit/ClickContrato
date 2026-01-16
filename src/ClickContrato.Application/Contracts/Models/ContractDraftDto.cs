namespace ClickContrato.Application.Contracts.Models;

public sealed record ContractDraftDto(
    Guid Id,
    string TemplateCode,
    int TemplateVersion,
    Dictionary<string, string> Fields
);


