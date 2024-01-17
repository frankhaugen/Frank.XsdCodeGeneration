using System.Xml.Schema;
using System.Xml.Serialization;

namespace Frank.XsdCodeGeneration;

public static class XmlSchemaExtensions
{
    public static XmlSchemas ToSchemas(this XmlSchemaSet schemaSet)
    {
        var schemas = new XmlSchemas();
        foreach (XmlSchema schema in schemaSet.Schemas())
        {
            var cleanedSchema = CleanupSchemaNamespace(schema);
            schemas.Add(schema);
        }
        return schemas;
    }
    
    private static XmlSchema CleanupSchemaNamespace(XmlSchema schema)
    {
        if (schema.TargetNamespace == null) return schema;
        
        var cleanNamespace = CleanUpNamespace(schema.TargetNamespace);
        schema.TargetNamespace = cleanNamespace;

        return schema;
    }

    private static string CleanUpNamespace(string @namespace)
    {
        var segments = @namespace.Split(':');
        var lastSegment = segments.Last();
        lastSegment = new string(lastSegment.Where(char.IsLetter).ToArray());
        if (lastSegment.Length == 0)
        {
            lastSegment = new string(segments[^2].Where(char.IsLetter).ToArray());
        }
        return lastSegment;
    }
}