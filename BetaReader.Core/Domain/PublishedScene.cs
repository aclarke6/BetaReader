namespace BetaReader.Core.Domain;

public sealed class PublishedScene
{
    public string ScrivenerId
    {
        get;
    }
    public string Title
    {
        get;
    }
    public string HtmlContent
    {
        get;
    }

    public PublishedScene(string scrivenerId, string title, string htmlContent)
    {
        ScrivenerId = scrivenerId;
        Title = title;
        HtmlContent = htmlContent;
    }
}