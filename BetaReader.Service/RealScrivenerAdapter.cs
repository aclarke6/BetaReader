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

            var binder = xml.Root!
                .Element("Binder")!
                .Elements("BinderItem")
                .Select(ParseBinderItem)
                .ToList();

            return binder;
        }

        private static ScrivenerDocument ParseBinderItem(XElement element)
        {
            var id = element.Attribute("UUID")!.Value;

            var title = element.Element("Title")?.Value ?? "(Untitled)";

            var children = element.Elements("Children")
                .Elements("BinderItem")
                .Select(ParseBinderItem)
                .ToList();

            return new ScrivenerDocument(id, title, children);
        }

        public Snapshot? GetLatestSnapshot(string vaultPath, string scrivenerId)
            => throw new NotImplementedException();

        public bool IsEligible(string vaultPath, string scrivenerId)
            => throw new NotImplementedException();
    }
}
