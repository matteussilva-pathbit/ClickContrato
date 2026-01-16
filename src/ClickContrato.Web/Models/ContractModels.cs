using System.Text.Json.Serialization;

namespace ClickContrato.Web.Models;

public sealed record ContractTemplateFieldDto(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("label")] string Label,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("required")] bool Required,
    [property: JsonPropertyName("placeholder")] string? Placeholder,
    [property: JsonPropertyName("maxLength")] int? MaxLength
);

public sealed record ContractTemplateSummaryDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("version")] int Version,
    [property: JsonPropertyName("fields")] List<ContractTemplateFieldDto> Fields
);

public sealed record CreateDraftRequest(
    [property: JsonPropertyName("templateCode")] string TemplateCode,
    [property: JsonPropertyName("templateVersion")] int? TemplateVersion
);

public sealed record ContractDraftDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("templateCode")] string TemplateCode,
    [property: JsonPropertyName("templateVersion")] int TemplateVersion,
    [property: JsonPropertyName("fields")] Dictionary<string, string> Fields
);

public sealed record UpdateDraftFieldsRequest(
    [property: JsonPropertyName("fields")] Dictionary<string, string> Fields
);

public sealed record PreviewResponse([property: JsonPropertyName("preview")] string Preview);


