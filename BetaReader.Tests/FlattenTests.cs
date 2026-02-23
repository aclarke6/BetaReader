using BetaReader.Core.Domain;
using BetaReader.Service.Sync;
using Xunit;

namespace BetaReader.Tests;

public sealed class FlattenTests
{
    [Fact]
    public void Flatten_Returns_All_Documents_In_DepthFirst_Order()
    {
        // Arrange: build a nested binder structure
        var doc3 = new ScrivenerDocument("3", "Scene 3", Array.Empty<ScrivenerDocument>());
        var folder = new ScrivenerDocument("folder", "Folder", new[] { doc3 });
        var doc1 = new ScrivenerDocument("1", "Scene 1", Array.Empty<ScrivenerDocument>());
        var doc2 = new ScrivenerDocument("2", "Scene 2", Array.Empty<ScrivenerDocument>());

        var root = new ScrivenerDocument("root", "Root", new[] { doc1, doc2, folder });

        // Act
        var flattened = SyncOrchestrator.Flatten(new[] { root }).ToList();

        // Assert: depth‑first order
        Assert.Collection(
            flattened,
            d => Assert.Equal("root", d.ScrivenerId),
            d => Assert.Equal("1", d.ScrivenerId),
            d => Assert.Equal("2", d.ScrivenerId),
            d => Assert.Equal("folder", d.ScrivenerId),
            d => Assert.Equal("3", d.ScrivenerId)
        );
    }
}