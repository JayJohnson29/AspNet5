using System.Text;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;

namespace Inde;
public static class ZipService
{

    public static XmlDocument Compress(XmlDocument xmlDocToCompress)
    {
        var xmlDoc = new XmlDocument();
        var docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
        xmlDoc.AppendChild(docNode);


        var compressedXmlDocNode = xmlDoc.CreateElement("CompressedNode");
        var compressedByteArray = Zip(xmlDocToCompress.OuterXml);
        compressedXmlDocNode.InnerText = Convert.ToBase64String(compressedByteArray);
        xmlDoc.AppendChild(compressedXmlDocNode);

        return xmlDoc;
    }

    public static byte[] Zip(string stringToZip) => Zip(Encoding.UTF8.GetBytes(stringToZip));

    public static byte[] Zip(byte[] inputByteArray)
    {
        var ms = new MemoryStream();
        var zipOut = new ZipOutputStream(ms);
        var zipEntry = new ZipEntry("ZippedFile");
        zipOut.PutNextEntry(zipEntry);
        zipOut.SetLevel(9);
        zipOut.Write(inputByteArray, 0, inputByteArray.Length);
        zipOut.Finish();
        zipOut.Close();
        return ms.ToArray();
    }

}


