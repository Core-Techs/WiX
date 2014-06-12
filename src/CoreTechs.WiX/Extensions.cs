using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Logos.Utility;

namespace CoreTechs.WiX
{
    static class Extensions
    {
        /// <summary>
        /// Converts the objects string representation to a guid.
        /// </summary>
        public static Guid ToGuid(this object value)
        {
            if (value == null) throw new ArgumentNullException("value");
            return GuidUtility.Create(Constants.GuidNamespace, value.ToString());
        }

        public static bool AreSame(this DirectoryInfo a, DirectoryInfo b)
        {
            if (!a.Name.Equals(b.Name, StringComparison.OrdinalIgnoreCase))
                return false;

            if (a.Parent == null && b.Parent == null)
                return true;

            if (a.Parent == null || b.Parent == null)
                return false;

            return a.Parent.FullName.Equals(b.Parent.FullName, StringComparison.OrdinalIgnoreCase);
        }

        public static string GetRelativePathFrom(this FileSystemInfo to, FileSystemInfo from)
        {
            return from.GetRelativePathTo(to);
        }

        public static string GetRelativePathTo(this FileSystemInfo from, FileSystemInfo to)
        {
            Func<FileSystemInfo, string> getPath = fsi =>
            {
                var d = fsi as DirectoryInfo;
                return d == null ? fsi.FullName : d.FullName.TrimEnd('\\') + "\\";
            };

            var fromPath = getPath(from);
            var toPath = getPath(to);

            var fromUri = new Uri(fromPath);
            var toUri = new Uri(toPath);

            var relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }

        public static FileInfo GetFile(this DirectoryInfo dir, string name)
        {
            return new FileInfo(Path.Combine(dir.FullName, name));
        }

        public static IEnumerable<byte> EnumerateBytes(this FileInfo file)
        {
            if (file == null) throw new ArgumentNullException("file");

            using (var r = file.OpenRead())
            {
                int b;
                while ((b = r.ReadByte()) != -1)
                    yield return (byte)b;

                r.Close();
            }
        }

        public static FileSystemInfo CreateFileSystemInfoFromPath(this string path)
        {
            return path.IsDirectoryPath() ? (FileSystemInfo) new DirectoryInfo(path) : new FileInfo(path);
        }

        public static bool IsDirectoryPath(this string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            path = path.Trim();

            if (Directory.Exists(path))
                return true;

            if (System.IO.File.Exists(path))
                return false;

            // neither file nor directory exists. guess intention

            // if has trailing slash then it's a directory
            if (new[] { "\\", "/" }.Any(x => path.EndsWith(x)))
                return true; // ends with slash

            // has if extension then its a file; directory otherwise
            return string.IsNullOrWhiteSpace(Path.GetExtension(path));
        }

        public static bool IsFilePath(this string path)
        {
            return !path.IsDirectoryPath();
        }

        /// <summary>
        /// Copies the file to the destination directory.
        /// </summary>
        /// <returns>Returns the destination file info.</returns>
        public static FileInfo CopyTo(this FileInfo file, DirectoryInfo directory, bool overwrite = false)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (directory == null) throw new ArgumentNullException("directory");
            directory.Create();
            var dest = directory.GetFile(file.Name);
            file.CopyTo(dest.FullName, overwrite);
            dest.Refresh();
            return dest;
        }
    }
}