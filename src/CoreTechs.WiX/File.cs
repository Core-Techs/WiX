using System;
using System.IO;
using System.Xml.Linq;
using CoreTechs.Common;

namespace CoreTechs.WiX
{
    public class File
    {
        private readonly Harvester _harvester;
        public FileInfo Info { get; private set; }
        readonly Lazy<string> _pathRelativeToRoot;
        readonly Lazy<string> _id;
        readonly Lazy<string> _hash;
        readonly Lazy<Guid> _guid;

        public string PathRelativeToRoot { get { return _pathRelativeToRoot.Value; } }
        public string Hash { get { return _hash.Value; } }
        public string Id { get { return _id.Value; } }
        public Guid Guid { get { return _guid.Value; } }

        internal File(FileInfo fileInfo, Harvester harvester)
        {
            if (fileInfo == null) throw new ArgumentNullException("fileInfo");
            if (harvester == null) throw new ArgumentNullException("harvester");
            Info = fileInfo;
            _harvester = harvester;
            _hash = new Lazy<string>(() => Info.ComputeFileHash());
            _pathRelativeToRoot = new Lazy<string>(() => _harvester.Directory.GetRelativePathTo(Info));
            _guid = new Lazy<Guid>(() => PathRelativeToRoot.ToUpperInvariant().ToGuid());
            _id = new Lazy<string>(() => "FILE_" + Guid.ToString("N").ToUpperInvariant());
        }

        public XElement ToXml()
        {
            var ns = Constants.WixNs;
            return new XElement(ns + "File",
                new XAttribute("Id", Id),
                new XAttribute("KeyPath", "yes"),
                new XAttribute("Name", Info.Name));
        }
    }
}