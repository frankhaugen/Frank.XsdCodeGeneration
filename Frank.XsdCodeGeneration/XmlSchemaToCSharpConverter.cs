using System.Text;
using System.Xml.Schema;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.XsdCodeGeneration;

public class XmlSchemaToCSharpConverter
{
    public string Convert(XmlSchemaSet schemaSet)
    {
        var classes = GetClasses(schemaSet);
        // var structs = GetStructs(schemaSet);
        // var enums = GetEnums(schemaSet);
        // var interfaces = GetInterfaces(schemaSet);
        
        // var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Frank.Markdown"));
        return "";
    }
    
    // private IEnumerable<EnumDeclarationSyntax> GetEnums(XmlSchemaSet schemaSet)
    // {
    //     var enums = new List<EnumDeclarationSyntax>();
    //     foreach (XmlSchema schema in schemaSet.Schemas())
    //     {
    //         foreach (XmlSchemaSimpleType simpleType in schema.Items.OfType<XmlSchemaSimpleType>())
    //         {
    //             var name = simpleType.Name;
    //             var members = new List<EnumMemberDeclarationSyntax>();
    //             foreach (XmlSchemaEnumerationFacet facet in simpleType.Content as XmlSchemaSimpleTypeRestriction)
    //             {
    //                 var member = SyntaxFactory.EnumMemberDeclaration(SyntaxFactory.Identifier(facet.Value));
    //                 members.Add(member);
    //             }
    //             var @enum = SyntaxFactory.EnumDeclaration(name).WithMembers(SyntaxFactory.SeparatedList(members));
    //             enums.Add(@enum);
    //         }
    //     }
    //     return enums;
    // }

    private static IEnumerable<ClassDeclarationSyntax> GetClasses(XmlSchemaSet schemaSet) 
        => (from XmlSchema schema in schemaSet.Schemas() from complexType in schema.Items.OfType<XmlSchemaComplexType>() select GetClass(complexType)).ToList();

    private static ClassDeclarationSyntax GetClass(XmlSchemaComplexType complexType)
    {
        var members = new List<MemberDeclarationSyntax>();
        foreach (XmlSchemaAttribute attribute in complexType.Attributes)
        {
            var name = attribute.Name;
            var type = attribute.SchemaTypeName.Name;
            var member = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), SyntaxFactory.Identifier(name));
            members.Add(member);
        }
        foreach (XmlSchemaElement element in complexType.Attributes)
        {
            var name = element.Name;
            var type = element.SchemaTypeName.Name;
            var member = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), SyntaxFactory.Identifier(name));
            members.Add(member);
        }
        
        return SyntaxFactory.ClassDeclaration(complexType.Name).WithMembers(SyntaxFactory.List(members));
    }
    
    
}