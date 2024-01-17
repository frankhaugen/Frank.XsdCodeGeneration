using System.Text;
using System.Xml.Schema;

namespace Frank.XsdCodeGeneration;

public class XsdWalkerLegacy
{
    private StringBuilder? _stringBuilder;
    
    public string Walk(XmlSchema schema)
    {
        _stringBuilder = new StringBuilder();
        
        foreach (var item in schema.Elements.Values)
        {
            switch (item)
            {

                case XmlSchemaSimpleType simpleType:
                    Walk(simpleType);
                    break;
                case XmlSchemaElement element:
                    Walk(element);
                    break;
                case XmlSchemaComplexType complexType:
                    Walk(complexType);
                    break;
                default:
                    throw new NotSupportedException("Unsupported element type.");
            }
        }

        return _stringBuilder.ToString();
    }

    private void Walk(XmlSchemaElement element)
    {
        if (element.SchemaType is XmlSchemaComplexType complexType)
        {
            Walk(complexType);
        }
        else if (element.SchemaType is XmlSchemaSimpleType simpleType)
        {
            Walk(simpleType);
        }
    }

    private void Walk(XmlSchemaComplexType complexType)
    {

        if (complexType.Attributes.Count > 0)
        {
            _stringBuilder.AppendLine("using System;");
            _stringBuilder.AppendLine("using System.Xml.Serialization;");
            _stringBuilder.AppendLine();

        }
        _stringBuilder.AppendLine($"namespace {complexType.QualifiedName.Namespace};");

        foreach (var attribute in complexType.Attributes)
        {
            
            _stringBuilder.AppendLine($"    [XmlAttribute(\"{attribute}\")]");
        }
        _stringBuilder.AppendLine($"public class {complexType.Name}");
        _stringBuilder.AppendLine("{");

            switch (complexType.Particle)
            {
                case XmlSchemaElement element:
                    Walk(element);
                    break;
                case XmlSchemaSequence sequence:
                    Walk(sequence);
                    break;
                case XmlSchemaChoice choice:
                    Walk(choice);
                    break;
                case XmlSchemaAny any:
                    Walk(any);
                    break;
                default:
                    throw new NotSupportedException("Unsupported particle type.");
            }
        
        _stringBuilder.AppendLine("}");
    }

    private void Walk(XmlSchemaAny any)
    {
        _stringBuilder.AppendLine($"    [XmlAnyElement]");
        _stringBuilder.AppendLine($"    public XmlElement[] {any.Id} {{ get; set; }}");
    }

    private void Walk(XmlSchemaChoice choiceInput)
    {
        foreach (var item in choiceInput.Items)
        {
            switch(item)
            {
                case XmlSchemaElement element:
                    Walk(element);
                    break;
                case XmlSchemaChoice choice:
                    Walk(choice);
                    break;
                case XmlSchemaSequence sequence:
                    Walk(sequence);
                    break;
                case XmlSchemaAny any:
                    Walk(any);
                    break;
                default:
                    throw new NotSupportedException("Unsupported particle type.");
            }
        }
    }

    private void Walk(XmlSchemaSequence sequenceInput)
    {
        foreach (var item in sequenceInput.Items)
        {
            switch (item)
            {
                case XmlSchemaElement element:
                    Walk(element);
                    break;
                case XmlSchemaChoice choice:
                    Walk(choice);
                    break;
                case XmlSchemaSequence sequence:
                    Walk(sequence);
                    break;
                case XmlSchemaAny any:
                    Walk(any);
                    break;
                default:
                    throw new NotSupportedException("Unsupported particle type.");
            }
        }
    }

    private void Walk(XmlSchemaSimpleType simpleType)
    {
        _stringBuilder.AppendLine($"public {simpleType.Name} {simpleType.Name} {{ get; set; }}");
    }
}