using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Inde;

public class Utf8StringWriter : StringWriter
{
    public override Encoding Encoding => Encoding.UTF8;
}
public class XmlService
{
    public static string SerializeObject<T>(T dataToSerialize)
    {
        if (dataToSerialize == null)
            return string.Empty;

        var xmlserializer = new XmlSerializer(typeof(T));
        var stringWriter = new Utf8StringWriter();
        using (var writer = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented })
        {
            xmlserializer.Serialize(writer, dataToSerialize);
            return stringWriter.ToString();
        }
    }
}
