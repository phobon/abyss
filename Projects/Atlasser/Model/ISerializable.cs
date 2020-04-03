using System.Xml;

namespace Atlasser.Model
{
    public interface ISerializable
    {
        void Serialize(XmlWriter writer);
    }
}
