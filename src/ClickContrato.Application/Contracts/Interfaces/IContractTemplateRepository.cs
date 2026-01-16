namespace ClickContrato.Application.Contracts.Interfaces;

public interface IContractTemplateRepository
{
    Task<IReadOnlyList<ContractTemplate>> ListAsync(CancellationToken ct);
    Task<ContractTemplate?> FindByCodeAsync(string code, int? version, CancellationToken ct);
    Task<ContractTemplate?> FindByIdAsync(Guid id, int? version, CancellationToken ct);
}


