namespace BetaReader.Service.Sync;

using BetaReader.Interfaces;
using BetaReader.Core.Domain;

public sealed class SyncOrchestrator
{
    private readonly IScrivenerAdapter _scrivener;
    private readonly IWebsiteApi _website;
    private readonly IVaultRegistry _vaults;
    private readonly ILogger _log;

    public SyncOrchestrator(
        IScrivenerAdapter scrivener,
        IWebsiteApi website,
        IVaultRegistry vaults,
        ILogger log)
    {
        _scrivener = scrivener;
        _website = website;
        _vaults = vaults;
        _log = log;
    }

    public async Task PollAsync(CancellationToken ct)
    {
        foreach (var vault in _vaults.GetAll())
        {
            await SyncVaultAsync(vault, ct);
        }
    }

    private async Task SyncVaultAsync(Vault vault, CancellationToken ct)
    {
        var binder = _scrivener.GetBinderHierarchy(vault.Path);

        var scenes = new List<PublishedScene>();

        foreach (var doc in Flatten(binder))
        {
            if (!_scrivener.IsEligible(vault.Path, doc.ScrivenerId))
                continue;

            var snapshot = _scrivener.GetLatestSnapshot(vault.Path, doc.ScrivenerId);
            if (snapshot == null)
                continue;

            var html = RtfToHtml(snapshot.RtfContent); // temporary stub

            scenes.Add(new PublishedScene(doc.ScrivenerId, doc.Title, html));
        }

        await _website.PublishDocumentsAsync(vault.Id, scenes, ct);
    }

    internal static IEnumerable<ScrivenerDocument> Flatten(IEnumerable<ScrivenerDocument> docs)
    {
        foreach (var d in docs)
        {
            yield return d;
            foreach (var c in Flatten(d.Children))
                yield return c;
        }
    }

    private static string RtfToHtml(string rtf)
    {
        return "<p>RTF conversion pending</p>";
    }
}