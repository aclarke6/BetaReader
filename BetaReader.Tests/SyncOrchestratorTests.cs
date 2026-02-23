using BetaReader.Core.Domain;
using BetaReader.Interfaces;
using BetaReader.Service.Sync;
using Moq;
using System.Timers;
using Xunit;

namespace BetaReader.Tests
{
    public sealed class SyncOrchestratorTests
    {
        [Fact]
        public async Task Poll_Publishes_Eligible_Documents()
        {
            var scrivener = new Mock<IScrivenerAdapter>();
            var website = new Mock<IWebsiteApi>();
            var vaults = new Mock<IVaultRegistry>();
            var log = new Mock<ILogger>();

            var vault = new Vault(Guid.NewGuid(), "C:\\TestVault");
            vaults.Setup(v => v.GetAll()).Returns(new[] { vault });

            var doc = new ScrivenerDocument("1", "Scene 1", Array.Empty<ScrivenerDocument>());
            scrivener.Setup(s => s.GetBinderHierarchy(vault.Path)).Returns(new[] { doc });
            scrivener.Setup(s => s.IsEligible(vault.Path, "1")).Returns(true);
            scrivener.Setup(s => s.GetLatestSnapshot(vault.Path, "1"))
                     .Returns(new Snapshot("1", DateTime.UtcNow, "{\\rtf1 test}"));

            var orchestrator = new SyncOrchestrator(scrivener.Object, website.Object, vaults.Object, log.Object);

            await orchestrator.PollAsync(CancellationToken.None);

            website.Verify(w => w.PublishDocumentsAsync(
                vault.Id,
                It.Is<IEnumerable<PublishedScene>>(scenes => scenes.Any()),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}