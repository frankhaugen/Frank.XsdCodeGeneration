using System.Xml;
using System.Xml.Serialization;

namespace Frank.XsdCodeGeneration;

public static class XmlSchemasExtensions
{
    public static XmlTypeMapping GetMapping(this XmlSchemas schemas, XmlQualifiedName @namespace)
    {
        var schemaImporter = new XmlSchemaImporter(schemas);
        var mapping = schemaImporter.ImportSchemaType(@namespace);
        return mapping;
    }
}