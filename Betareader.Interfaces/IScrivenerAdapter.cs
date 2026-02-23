namespace BetaReader.Interfaces;

using BetaReader.Core.Domain;

public interface IScrivenerAdapter
{
    IReadOnlyList<ScrivenerDocument> GetBinderHierarchy(string vaultPath);
    Snapshot? GetLatestSnapshot(string vaultPath, string scrivenerId);
    bool IsEligible(string vaultPath, string scrivenerId);
}