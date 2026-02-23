namespace BetaReader.Interfaces;

using BetaReader.Core.Domain;

public interface IWebsiteApi
{
    Task PublishDocumentsAsync(Guid vaultId, IEnumerable<PublishedScene> scenes, CancellationToken ct);
}