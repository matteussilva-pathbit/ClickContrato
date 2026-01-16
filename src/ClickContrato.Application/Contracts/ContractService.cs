namespace ClickContrato.Application.Contracts;

public sealed class ContractService
{
    private readonly IContractTemplateRepository _templates;
    private readonly IContractDraftRepository _drafts;

    public ContractService(IContractTemplateRepository templates, IContractDraftRepository drafts)
    {
        _templates = templates;
        _drafts = drafts;
    }

    public async Task<IReadOnlyList<ContractTemplateSummaryDto>> ListTemplatesAsync(CancellationToken ct = default)
    {
        var templates = await _templates.ListAsync(ct);

        return templates
            .OrderBy(t => t.Name)
            .Select(t => new ContractTemplateSummaryDto(
                t.Id,
                t.Code,
                t.Name,
                t.Version,
                t.Fields.Select(f => new ContractTemplateFieldDto(
                    f.Key,
                    f.Label,
                    f.Type.ToString(),
                    f.Required,
                    f.Placeholder,
                    f.MaxLength
                )).ToList()
            ))
            .ToList();
    }

    public async Task<Result<ContractDraftDto>> CreateDraftAsync(CreateDraftRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.TemplateCode))
            return Result<ContractDraftDto>.Failure("invalid_template", "TemplateCode é obrigatório.");

        var template = await _templates.FindByCodeAsync(request.TemplateCode, request.TemplateVersion, ct);
        if (template is null)
            return Result<ContractDraftDto>.Failure("template_not_found", "Template não encontrado.");

        var draft = new ContractDraft(
            id: Guid.NewGuid(),
            templateId: template.Id,
            templateCode: template.Code,
            templateVersion: template.Version,
            status: ContractDraftStatus.Draft,
            fieldValues: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
            createdAtUtc: DateTime.UtcNow
        );

        await _drafts.AddAsync(draft, ct);

        return Result<ContractDraftDto>.Success(new ContractDraftDto(draft.Id, draft.TemplateCode, draft.TemplateVersion, draft.FieldValues));
    }

    public async Task<Result<ContractDraftDto>> UpdateDraftFieldsAsync(Guid draftId, UpdateDraftFieldsRequest request, CancellationToken ct = default)
    {
        var draft = await _drafts.FindByIdAsync(draftId, ct);
        if (draft is null)
            return Result<ContractDraftDto>.Failure("draft_not_found", "Rascunho não encontrado.");

        if (request.Fields is null || request.Fields.Count == 0)
            return Result<ContractDraftDto>.Failure("invalid_fields", "Fields não pode ser vazio.");

        draft.SetFields(request.Fields);
        await _drafts.UpdateAsync(draft, ct);

        return Result<ContractDraftDto>.Success(new ContractDraftDto(draft.Id, draft.TemplateCode, draft.TemplateVersion, draft.FieldValues));
    }

    public async Task<Result<string>> GetDraftPreviewAsync(Guid draftId, CancellationToken ct = default)
    {
        var draft = await _drafts.FindByIdAsync(draftId, ct);
        if (draft is null)
            return Result<string>.Failure("draft_not_found", "Rascunho não encontrado.");

        var template = await _templates.FindByIdAsync(draft.TemplateId, draft.TemplateVersion, ct);
        if (template is null)
            return Result<string>.Failure("template_not_found", "Template não encontrado.");

        var rendered = RenderTemplate(template, draft.FieldValues);
        return Result<string>.Success(rendered);
    }

    private static string RenderTemplate(ContractTemplate template, Dictionary<string, string> fields)
    {
        var output = template.Body;

        // placeholder especial
        output = output.Replace("{{DataHoje}}", DateTime.Now.ToString("dd/MM/yyyy"), StringComparison.OrdinalIgnoreCase);

        foreach (var f in template.Fields)
        {
            var value = fields.TryGetValue(f.Key, out var v) ? v : string.Empty;
            output = output.Replace($"{{{{{f.Key}}}}}", value ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        return output;
    }
}


