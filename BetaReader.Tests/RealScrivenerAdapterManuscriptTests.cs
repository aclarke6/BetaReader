using BetaReader.Service;
using Xunit;

namespace BetaReader.Tests;

public sealed class RealScrivenerAdapterManuscriptTests
{
    [Fact]
    public void GetBinderHierarchy_Returns_Only_Manuscript_Root()
    {
        // Arrange
        var adapter = new RealScrivenerAdapter();
        var path = @"C:\Users\alast\Dropbox\Apps\Scrivener\Test.scriv";

        // Act
        var binder = adapter.GetBinderHierarchy(path);

        // Assert
        var root = Assert.Single(binder);
        Assert.Equal("Manuscript", root.Title);
        Assert.NotEmpty(root.Children);
    }
}