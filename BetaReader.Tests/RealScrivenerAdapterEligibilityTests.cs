using BetaReader.Core.Domain;
using BetaReader.Service;
using Xunit;

namespace BetaReader.Tests;

public sealed class RealScrivenerAdapterEligibilityTests
{
    private const string VaultPath =
        @"C:\Users\alast\Dropbox\Apps\Scrivener\Test.scriv";

    [Fact]
    public void IsEligible_Returns_True_For_FirstDraft_MarkerScene()
    {
        // Arrange
        var adapter = new RealScrivenerAdapter();
        var binder = adapter.GetBinderHierarchy(VaultPath);
        var docId = FindDocumentIdByMarker(adapter, binder, "Scene ID: B1-P1-C1-S2");

        // Act
        var result = adapter.IsEligible(VaultPath, docId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEligible_Returns_False_For_InProgress_MarkerScene()
    {
        // Arrange
        var adapter = new RealScrivenerAdapter();
        var binder = adapter.GetBinderHierarchy(VaultPath);
        var docId = FindDocumentIdByMarker(adapter, binder, "Scene ID: B1-P2-C3-S1");

        // Act
        var result = adapter.IsEligible(VaultPath, docId);

        // Assert
        Assert.False(result);
    }

    private static string FindDocumentIdByMarker(
        RealScrivenerAdapter adapter,
        IEnumerable<ScrivenerDocument> binder,
        string marker)
    {
        foreach (var doc in Flatten(binder))
        {
            var snapshot = adapter.GetLatestSnapshot(VaultPath, doc.ScrivenerId);
            if (snapshot is null)
                continue;

            if (snapshot.RtfContent.Contains(marker, StringComparison.Ordinal))
                return doc.ScrivenerId;
        }

        throw new InvalidOperationException($"Could not find a document containing marker: {marker}");
    }

    private static IEnumerable<ScrivenerDocument> Flatten(IEnumerable<ScrivenerDocument> docs)
    {
        foreach (var d in docs)
        {
            yield return d;
            foreach (var c in Flatten(d.Children))
                yield return c;
        }
    }
}