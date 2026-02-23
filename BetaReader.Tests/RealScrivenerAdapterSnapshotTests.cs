using BetaReader.Service;
using Xunit;

namespace BetaReader.Tests;

public sealed class RealScrivenerAdapterSnapshotTests
{
    private const string VaultPath =
        @"C:\Users\alast\Dropbox\Apps\Scrivener\Test.scriv";

    private const string SceneUuid =
        "A9C97B44-46C8-4CA8-8F28-B8C0606A58EF";

    [Fact]
    public void GetLatestSnapshot_Returns_Rtf_For_Existing_Scene()
    {
        // Arrange
        var adapter = new RealScrivenerAdapter();

        // Act
        var snapshot = adapter.GetLatestSnapshot(VaultPath, SceneUuid);

        // Assert
        Assert.NotNull(snapshot);
        Assert.Equal(SceneUuid, snapshot!.DocumentId);
        Assert.False(string.IsNullOrWhiteSpace(snapshot.SnapshotId));
        Assert.Contains(@"{\rtf", snapshot.RtfContent);
    }
}
