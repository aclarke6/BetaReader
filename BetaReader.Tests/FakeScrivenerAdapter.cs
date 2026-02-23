using System;
using System.Collections.Generic;
using BetaReader.Core.Domain;
using BetaReader.Interfaces;

namespace BetaReader.Tests;

internal sealed class FakeScrivenerAdapter : IScrivenerAdapter
{
    public IReadOnlyList<ScrivenerDocument> GetBinderHierarchy(string vaultPath)
    {
        // Deterministic fake binder tree
        return new[]
        {
            new ScrivenerDocument(
                "root",
                "Root",
                new[]
                {
                    new ScrivenerDocument("1", "Scene 1", Array.Empty<ScrivenerDocument>()),
                    new ScrivenerDocument("2", "Scene 2", Array.Empty<ScrivenerDocument>()),
                    new ScrivenerDocument(
                        "folder-1",
                        "Folder 1",
                        new[]
                        {
                            new ScrivenerDocument("3", "Scene 3", Array.Empty<ScrivenerDocument>())
                        })
                })
        };
    }

    public Snapshot? GetLatestSnapshot(string vaultPath, string scrivenerId)
    {
        return new Snapshot(
            documentId: scrivenerId,
            snapshotId: $"snap-{scrivenerId}",
            timestampUtc: new DateTime(2024, 01, 01, 12, 00, 00, DateTimeKind.Utc),
            rtfContent: "{\\rtf1 Fake content for " + scrivenerId + "}"
        );
    }

    public bool IsEligible(string vaultPath, string scrivenerId)
    {
        // In the fake, everything is eligible
        return true;
    }
}