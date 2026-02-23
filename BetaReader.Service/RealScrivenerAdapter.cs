using BetaReader.Core.Domain;
using BetaReader.Interfaces;
using System.Xml.Linq;

namespace BetaReader.Service
{
    public sealed class RealScrivenerAdapter : IScrivenerAdapter
    {
        public IReadOnlyList<ScrivenerDocument> GetBinderHierarchy(string vaultPath)
        {
            var scrivxPath = Directory.GetFiles(vaultPath, "*.scrivx").Single();

            var xml = XDocument.Load(scrivxPath);

            var binderItems = xml.Root!
                .Element("Binder")!
                .Elements("BinderItem")
                .Select(ParseBinderItem)
                .ToList();

            var manuscript = binderItems.SingleOrDefault(x => x.Title == "Manuscript");

            if (manuscript is null)
                throw new InvalidOperationException("Scrivener project has no Manuscript root.");

            return [manuscript];
        }

        private static ScrivenerDocument ParseBinderItem(XElement element)
        {
            var id = element.Attribute("UUID")?.Value
                ?? throw new InvalidOperationException("BinderItem missing UUID attribute.");

            var title = element.Element("Title")?.Value ?? "(Untitled)";

            var children = element.Elements("Children")
                .Elements("BinderItem")
                .Select(ParseBinderItem)
                .ToList();

            return new ScrivenerDocument(id, title, children);
        }

        public Snapshot? GetLatestSnapshot(string vaultPath, string scrivenerId)
        {
            // scrivenerId is the BinderItem UUID
            var contentPath = Path.Combine(
                vaultPath,
                "Files",
                "Data",
                scrivenerId,
                "content.rtf");

            if (!File.Exists(contentPath))
                return null;

            var rtf = File.ReadAllText(contentPath);

            // v1: deterministic snapshot id until we parse real Scrivener snapshots
            var snapshotId = $"content-{scrivenerId}";

            var timestampUtc = File.GetLastWriteTimeUtc(contentPath);

            return new Snapshot(
                documentId: scrivenerId,
                snapshotId: snapshotId,
                timestampUtc: timestampUtc,
                rtfContent: rtf);
        }

        public bool IsEligible(string vaultPath, string scrivenerId)
        {
            var scrivxPath = Directory.GetFiles(vaultPath, "*.scrivx").Single();
            var xml = XDocument.Load(scrivxPath);

            var binder = xml.Root?.Element("Binder");
            if (binder is null)
                return false;

            var item = binder
                .Descendants("BinderItem")
                .SingleOrDefault(x =>
                    string.Equals((string?) x.Attribute("UUID"), scrivenerId, StringComparison.OrdinalIgnoreCase));

            if (item is null)
                return false;

            var statusText = item.Element("MetaData")?.Element("StatusID")?.Value;
            if (!int.TryParse(statusText, out var statusId))
                return false;

            // Publishable statuses only (In Progress is NOT publishable)
            return statusId is 2 or 3 or 4 or 5;
        }
    }
}
