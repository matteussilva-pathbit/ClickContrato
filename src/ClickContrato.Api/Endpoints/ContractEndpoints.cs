namespace ClickContrato.Api.Endpoints;

public static class ContractEndpoints
{
    public static IEndpointRouteBuilder MapContractEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("")
            .WithTags("Contratos")
            .RequireAuthorization();

        group.MapGet("/contract-templates", async (ContractService contractService) =>
            Results.Ok(await contractService.ListTemplatesAsync()))
            .WithName("ListContractTemplates")
            .WithSummary("Listar templates disponíveis")
            .WithDescription("Retorna os tipos de contrato e os campos necessários para preencher cada um.")
            .Produces(StatusCodes.Status200OK);

        group.MapPost("/contracts/drafts", async (CreateDraftRequest request, ContractService contractService) =>
            {
                var result = await contractService.CreateDraftAsync(request);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(new { code = result.ErrorCode, message = result.ErrorMessage });
            })
            .WithName("CreateContractDraft")
            .WithSummary("Criar rascunho de contrato")
            .WithDescription("""
                             Cria um rascunho a partir de um template (ex.: 'prestacao-servicos' ou 'freelancer').
                             
                             Exemplo de body:
                             {
                               "templateCode": "prestacao-servicos",
                               "templateVersion": null
                             }
                             """)
            .Accepts<CreateDraftRequest>("application/json")
            .Produces<ContractDraftDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/contracts/drafts/{draftId:guid}/fields",
                async (Guid draftId, UpdateDraftFieldsRequest request, ContractService contractService) =>
                {
                    var result = await contractService.UpdateDraftFieldsAsync(draftId, request);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(new { code = result.ErrorCode, message = result.ErrorMessage });
                })
            .WithName("UpdateContractDraftFields")
            .WithSummary("Atualizar campos do rascunho")
            .WithDescription("""
                             Envia os campos preenchidos. As chaves devem bater com os 'fields.key' retornados em /contract-templates.
                             
                             Exemplo (prestacao-servicos):
                             {
                               "fields": {
                                 "ContratanteNome": "João",
                                 "ContratanteDocumento": "000.000.000-00",
                                 "ContratadoNome": "Maria",
                                 "ContratadoDocumento": "00.000.000/0001-00",
                                 "Objeto": "Criação de site",
                                 "Valor": "1500,00",
                                 "Prazo": "30 dias"
                               }
                             }
                             """)
            .Accepts<UpdateDraftFieldsRequest>("application/json")
            .Produces<ContractDraftDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/contracts/drafts/{draftId:guid}/preview",
                async (Guid draftId, ContractService contractService) =>
                {
                    var result = await contractService.GetDraftPreviewAsync(draftId);
                    return result.IsSuccess
                        ? Results.Ok(new { preview = result.Value })
                        : Results.NotFound(new { code = result.ErrorCode, message = result.ErrorMessage });
                })
            .WithName("PreviewContractDraft")
            .WithSummary("Gerar prévia (texto) do contrato")
            .WithDescription("Retorna o texto do contrato com as variáveis já substituídas. (MVP: apenas texto; PDF vem depois).")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}


