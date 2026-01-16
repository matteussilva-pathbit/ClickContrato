namespace ClickContrato.Domain.Contracts;

public sealed class ContractDraft
{
    public Guid Id { get; }
    public Guid TemplateId { get; }
    public string TemplateCode { get; }
    public int TemplateVersion { get; }
    public ContractDraftStatus Status { get; private set; }
    public Dictionary<string, string> FieldValues { get; }
    public DateTime CreatedAtUtc { get; }

    public ContractDraft(
        Guid id,
        Guid templateId,
        string templateCode,
        int templateVersion,
        ContractDraftStatus status,
        Dictionary<string, string> fieldValues,
        DateTime createdAtUtc)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id inválido.", nameof(id));
        if (templateId == Guid.Empty) throw new ArgumentException("TemplateId inválido.", nameof(templateId));
        if (string.IsNullOrWhiteSpace(templateCode)) throw new ArgumentException("TemplateCode é obrigatório.", nameof(templateCode));
        if (templateVersion <= 0) throw new ArgumentException("TemplateVersion inválida.", nameof(templateVersion));

        Id = id;
        TemplateId = templateId;
        TemplateCode = templateCode.Trim().ToLowerInvariant();
        TemplateVersion = templateVersion;
        Status = status;
        FieldValues = fieldValues ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        CreatedAtUtc = createdAtUtc;
    }

    public void SetFields(IDictionary<string, string> fields)
    {
        foreach (var (k, v) in fields)
        {
            FieldValues[k] = v;
        }
    }
}


