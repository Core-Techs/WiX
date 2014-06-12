using System;
using System.Xml.Linq;
using Logos.Utility;

namespace CoreTechs.WiX
{
    public static class Constants
    {
        public static readonly XNamespace WixNs = "http://schemas.microsoft.com/wix/2006/wi";
        public static readonly Guid GuidNamespace = GuidUtility.Create(GuidUtility.DnsNamespace, "core-techs.net");
    }
}