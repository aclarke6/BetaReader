namespace BetaReader.Core.Domain;

public sealed class Snapshot
{
    public string DocumentId
    {
        get;
    }
    public string SnapshotId
    {
        get;
    }
    public DateTime TimestampUtc
    {
        get;
    }
    public string RtfContent
    {
        get;
    }

    public Snapshot(string documentId, string snapshotId, DateTime timestampUtc, string rtfContent)
    {
        DocumentId = documentId;
        SnapshotId = snapshotId;
        TimestampUtc = timestampUtc;
        RtfContent = rtfContent;
    }
}