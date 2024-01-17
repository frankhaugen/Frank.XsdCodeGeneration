using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Frank.XsdCodeGeneration;

public static class XmlSchemaSetLoader
{
    public static XmlSchemaSet Load(params FileInfo[] files)
    {
        var set = new XmlSchemaSet();
        var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
        var readers = files.Select(f => XmlReader.Create(f.FullName, settings));

        set.XmlResolver = new XmlUrlResolver();
        set.ValidationEventHandler += (s, e) =>
        {
            var ex = e.Exception as Exception;
            while (ex != null)
            {
                // Log?.Invoke(ex.Message);
                ex = ex.InnerException;
            }
        };

        foreach (var reader in readers)
            set.Add(null, reader);

        return set;
    }
}
