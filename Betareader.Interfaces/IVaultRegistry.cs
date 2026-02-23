namespace BetaReader.Interfaces;

using BetaReader.Core.Domain;

public interface IVaultRegistry
{
    IReadOnlyList<Vault> GetAll();
    void Add(Vault vault);
    void Remove(Guid vaultId);
}