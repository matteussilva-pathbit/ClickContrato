namespace ClickContrato.Application.Contracts.Models;

public sealed record CreateDraftRequest(
    string TemplateCode,
    int? TemplateVersion = null
);


