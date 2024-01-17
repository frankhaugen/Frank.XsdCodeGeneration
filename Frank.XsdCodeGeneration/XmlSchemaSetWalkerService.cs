using System.Xml.Schema;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.XsdCodeGeneration;

public class XmlSchemaSetWalkerService
{
    public NamespaceDeclarationSyntax Walk(XmlSchemaSet schemaSet)
    {
        var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Frank.Unknown"));
        
        var classes = new List<ClassDeclarationSyntax>();
        var enums = new List<EnumDeclarationSyntax>();
        var interfaces = new List<InterfaceDeclarationSyntax>();
        
        foreach (XmlSchema schema in schemaSet.Schemas())
        {
            classes.AddRange(ExtractClasses(schema));
            enums.AddRange(ExtractEnums(schema));
            interfaces.AddRange(ExtractInterfaces(schema));
        }
        
        namespaceDeclaration = namespaceDeclaration.AddMembers(classes.ToArray());
        
        return namespaceDeclaration;
    }

    private IEnumerable<InterfaceDeclarationSyntax> ExtractInterfaces(XmlSchema schema)
    {
        var interfaces = new List<InterfaceDeclarationSyntax>();
        
        foreach (XmlSchemaComplexType complexType in schema.Items.OfType<XmlSchemaComplexType>())
        {
            var members = new List<MemberDeclarationSyntax>();
            foreach (var o in complexType.Attributes)
            {
                try
                {
                    var attribute = (XmlSchemaAttribute)o;
                    var name = attribute.Name;
                    var type = attribute.SchemaTypeName.Name;
                    var member = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), SyntaxFactory.Identifier(name));
                    members.Add(member);
                }
                catch (Exception e)
                {
                    
                }
            }
            foreach (var o in complexType.Attributes)
            {
                try
                {
                    var element = (XmlSchemaElement)o;
                    var name = element.Name;
                    var type = element.SchemaTypeName.Name;
                    var member = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), SyntaxFactory.Identifier(name));
                    members.Add(member);
                }
                catch (Exception e)
                {
                    
                }
            }
            var @interface = SyntaxFactory.InterfaceDeclaration(complexType.Name).WithMembers(SyntaxFactory.List(members));
            interfaces.Add(@interface);
        }
        
        return interfaces;
    }

    private IEnumerable<ClassDeclarationSyntax> ExtractClasses(XmlSchema schema)
    {
        var classes = new List<ClassDeclarationSyntax>();
        
        foreach (XmlSchemaComplexType complexType in schema.Items.OfType<XmlSchemaComplexType>())
        {
            var members = new List<MemberDeclarationSyntax>();
            foreach (var o in complexType.Attributes)
            {
                try
                {
                    var attribute = (XmlSchemaAttribute)o;
                    var name = attribute.Name;
                    var type = attribute.SchemaTypeName.Name;
                    var member = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), SyntaxFactory.Identifier(name));
                    members.Add(member);
                }
                catch (Exception e)
                {
                    
                }
            }
            foreach (var o in complexType.Attributes)
            {
                try
                {
                    var element = (XmlSchemaElement)o;
                    var name = element.Name;
                    var type = element.SchemaTypeName.Name;
                    var member = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), SyntaxFactory.Identifier(name));
                    members.Add(member);
                }
                catch (Exception e)
                {
                    
                }
            }
            var @class = SyntaxFactory.ClassDeclaration(complexType.Name).WithMembers(SyntaxFactory.List(members));
            classes.Add(@class);
        }
        
        return classes;
    }
    
    private IEnumerable<EnumDeclarationSyntax> ExtractEnums(XmlSchema schema)
    {
        var enums = new List<EnumDeclarationSyntax>();
        foreach (XmlSchemaSimpleType simpleType in schema.Items.OfType<XmlSchemaSimpleType>())
        {
            var name = simpleType.Name;
            var members = new List<EnumMemberDeclarationSyntax>();
            // TODO: Fix this
            // foreach (XmlSchemaEnumerationFacet facet in simpleType.Content as XmlSchemaSimpleTypeRestriction)
            // {
            //     var member = SyntaxFactory.EnumMemberDeclaration(SyntaxFactory.Identifier(facet.Value));
            //     members.Add(member);
            // }
            var @enum = SyntaxFactory.EnumDeclaration(name).WithMembers(SyntaxFactory.SeparatedList(members));
            enums.Add(@enum);
        }
        return enums;
    }
}