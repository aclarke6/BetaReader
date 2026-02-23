namespace BetaReader.Core.Domain;

public sealed class ScrivenerDocument
{
    public string ScrivenerId
    {
        get;
    }
    public string Title
    {
        get;
    }
    public IReadOnlyList<ScrivenerDocument> Children
    {
        get;
    }

    public ScrivenerDocument(string scrivenerId, string title, IEnumerable<ScrivenerDocument> children)
    {
        ScrivenerId = scrivenerId;
        Title = title;
        Children = children.ToList();
    }
}