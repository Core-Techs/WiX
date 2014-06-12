using System;
using System.Linq;
using System.Xml.Linq;

namespace CoreTechs.WiX
{
    public class Component
    {
        public File File { get; private set; }
        private readonly Harvester _harvester;
        private readonly Lazy<string> _id;
        public string Id { get { return _id.Value; } }

        internal Component(File file, Harvester harvester)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (harvester == null) throw new ArgumentNullException("harvester");
            File = file;
            _harvester = harvester;
            _id = new Lazy<string>(() => "CMP_" + File.Guid.ToString("N").ToUpperInvariant());
        }

        internal XElement InjectXml(XContainer container)
        {
            var ns = Constants.WixNs;
            XElement compXel;
            var xml = compXel = new XElement(ns + "Component",
                new XAttribute("Id", Id),
                new XAttribute("Guid", File.Guid),
                File.ToXml());

            // embed component within directory elements
            var dir = File.Info.Directory;
            while (!dir.AreSame(_harvester.Directory))
            {
                var relativePathTo = dir.GetRelativePathFrom(_harvester.Directory);

                var dirId = "DIR_" + relativePathTo.ToUpperInvariant().ToGuid().ToString("N").ToUpperInvariant();

                var dirXml = container.Descendants(ns + "Directory")
                    .SingleOrDefault(x => x.Attribute("Id").Value == dirId);

                if (dirXml == null)
                {
                    xml = new XElement(ns + "Directory",
                        new XAttribute("Id", dirId),
                        new XAttribute("Name", dir.Name),
                        xml);

                    dir = dir.Parent;
                }
                else
                {
                    dirXml.Add(xml);
                    return compXel;
                }
            }

            container.Add(xml);
            return compXel;
        }
    }
}