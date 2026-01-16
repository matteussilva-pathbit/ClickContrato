namespace ClickContrato.Infrastructure.Contracts;

public sealed class InMemoryContractDraftRepository : IContractDraftRepository
{
    private static readonly ConcurrentDictionary<Guid, ContractDraft> Drafts = new();

    public Task AddAsync(ContractDraft draft, CancellationToken ct)
    {
        Drafts[draft.Id] = draft;
        return Task.CompletedTask;
    }

    public Task<ContractDraft?> FindByIdAsync(Guid id, CancellationToken ct)
    {
        Drafts.TryGetValue(id, out var draft);
        return Task.FromResult(draft);
    }

    public Task UpdateAsync(ContractDraft draft, CancellationToken ct)
    {
        Drafts[draft.Id] = draft;
        return Task.CompletedTask;
    }
}


