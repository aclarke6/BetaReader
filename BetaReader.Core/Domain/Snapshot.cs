namespace BetaReader.Core.Domain;

public sealed class Snapshot
{
    public string ScrivenerId
    {
        get;
    }
    public DateTime Timestamp
    {
        get;
    }
    public string RtfContent
    {
        get;
    }

    public Snapshot(string scrivenerId, DateTime timestamp, string rtfContent)
    {
        ScrivenerId = scrivenerId;
        Timestamp = timestamp;
        RtfContent = rtfContent;
    }
}