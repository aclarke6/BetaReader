namespace BetaReader.Core.Domain;

public sealed class Vault
{
    public Guid Id
    {
        get;
    }
    public string Path
    {
        get;
    }

    public Vault(Guid id, string path)
    {
        Id = id;
        Path = path;
    }
}
