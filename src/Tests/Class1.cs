using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CoreTechs.WiX;
using NUnit.Framework;

namespace Tests
{
    public class Class1
    {
        [Test]
        public void Test1()
        {
            var h = new Harvester(@"D:\code\TrueCards\src\TrueCards\bin\Release", @"d:\users\roverby\mywixfile.wxs", "ProgramFilesDirectory","ProductComponents")
            {
                ExcludeExtensions = new[] {"pdb", "xml", "vshost.exe", "vshost.exe.config", "vshost.exe.manifest"}
            }

                .ModifyComponentsWhere(c => c.File.Info.Extension.Equals(".exe", StringComparison.OrdinalIgnoreCase),
                    (c, xe) => xe.Elements(Constants.WixNs + "File").Single().Add(new XAttribute("Checksum", "yes")));

            var xml = h.Harvest();
            Console.Write(xml.ToString());
        }

        [Test]
        public void RelPath()
        {
            var dir = new DirectoryInfo(@"c:\dir");
            var dir2 = new DirectoryInfo(@"c:\dir2");
            var file1 = new FileInfo(@"c:\dir2\file1.txt");
            var file2 = new FileInfo(@"c:\dir2\file2.txt");

            Console.WriteLine(dir.GetRelativePathTo(dir2));
        }
    }
}
