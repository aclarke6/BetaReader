using BetaReader.Interfaces;
using BetaReader.Core.Domain;
using Xunit;
using BetaReader.Service;

namespace BetaReader.Tests;

public sealed class RealScrivenerAdapterTests
{
    [Fact]
    public void GetBinderHierarchy_Returns_Documents_From_Real_Vault()
    {
        // Arrange
        var adapter = new RealScrivenerAdapter(); // you will implement this
        var path = @"C:\Users\alast\Dropbox\Apps\Scrivener\Test.scriv";

        // Act
        var binder = adapter.GetBinderHierarchy(path);

        // Assert
        Assert.NotNull(binder);
        Assert.NotEmpty(binder);
    }
}