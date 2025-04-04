using System.Xml;

namespace MC;

public class XmlFileDto
{
    public XmlDocument FileData { get; set; }
    public string FileName { get; set; }

}
public class FileDto
{
    public byte[] FileData { get; set; }
    public string FileName { get; set; }

}