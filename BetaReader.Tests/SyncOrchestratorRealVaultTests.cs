using BetaReader.Core.Domain;
using BetaReader.Interfaces;
using BetaReader.Service;
using BetaReader.Service.Sync;
using Xunit;

namespace BetaReader.Tests;

public sealed class SyncOrchestratorRealVaultTests
{
    private const string VaultPath = @"C:\Users\alast\Dropbox\Apps\Scrivener\Test.scriv";

    [Fact]
    public async Task PollAsync_Publishes_Only_Eligible_Manuscript_Scenes_From_Real_Vault()
    {
        // Arrange
        var vaultId = Guid.NewGuid();
        var vaults = new InMemoryVaultRegistry(new Vault(vaultId, VaultPath));
        var website = new SpyWebsiteApi();
        var log = new NullLogger();
        var scrivener = new RealScrivenerAdapter();

        var orchestrator = new SyncOrchestrator(scrivener, website, vaults, log);

        // Act
        await orchestrator.PollAsync(CancellationToken.None);

        // Assert
        Assert.Equal(vaultId, website.LastVaultId);
        Assert.NotNull(website.LastScenes);
        Assert.NotEmpty(website.LastScenes);

        // In Progress should not publish (you said B1-P1-C1-S1 is In Progress)
        Assert.DoesNotContain(
            website.LastScenes!,
            s => s.HtmlContent.Contains("Scene ID: B1-P1-C1-S1", StringComparison.Ordinal));

        // First Draft should publish (you said B1-P1-C1-S2 is First Draft)
        Assert.Contains(
            website.LastScenes!,
            s => s.HtmlContent.Contains("Scene ID: B1-P1-C1-S2", StringComparison.Ordinal));
    }

    private sealed class SpyWebsiteApi : IWebsiteApi
    {
        public Guid LastVaultId
        {
            get; private set;
        }
        public List<PublishedScene>? LastScenes
        {
            get; private set;
        }

        public Task PublishDocumentsAsync(Guid vaultId, IEnumerable<PublishedScene> scenes, CancellationToken ct)
        {
            LastVaultId = vaultId;
            LastScenes = scenes.ToList();
            return Task.CompletedTask;
        }
    }

    private sealed class InMemoryVaultRegistry : IVaultRegistry
    {
        private readonly List<Vault> _vaults;

        public InMemoryVaultRegistry(params Vault[] vaults)
        {
            _vaults = vaults.ToList();
        }

        public IReadOnlyList<Vault> GetAll() => _vaults;
        public void Add(Vault vault) => _vaults.Add(vault);
        public void Remove(Guid vaultId) => _vaults.RemoveAll(v => v.Id == vaultId);
    }

    private sealed class NullLogger : ILogger
    {
        public void Info(string message)
        {
        }
        public void Error(string message, Exception ex)
        {
        }
    }
}