using System.Xml.Linq;

namespace CoreTechs.WiX
{
    /// <summary>
    /// Modifies generated XML.
    /// </summary>
    public interface ITransformation
    {
        XDocument Transform(XDocument xml);
    }
}