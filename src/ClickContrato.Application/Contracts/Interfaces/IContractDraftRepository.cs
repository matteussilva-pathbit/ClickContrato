namespace ClickContrato.Application.Contracts.Interfaces;

public interface IContractDraftRepository
{
    Task AddAsync(ContractDraft draft, CancellationToken ct);
    Task<ContractDraft?> FindByIdAsync(Guid id, CancellationToken ct);
    Task UpdateAsync(ContractDraft draft, CancellationToken ct);
}


