using System.Collections.Generic;
using System.IO;
using CoreTechs.Common;

namespace CoreTechs.WiX
{
    internal class FilePathEqualityComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            return x.AreSame(y);
        }

        public int GetHashCode(FileInfo obj)
        {
            return obj.FullName.ToUpperInvariant().GetHashCode();
        }
    }
}