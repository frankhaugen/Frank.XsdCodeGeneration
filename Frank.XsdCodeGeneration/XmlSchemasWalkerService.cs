using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.XsdCodeGeneration;

public class XmlSchemasWalkerService
{
    public NamespaceDeclarationSyntax Walk(XmlSchemas schemas)
    {
        // var mappings = schemas.GetMapping();
        // var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(namespaceName));
        // foreach (XmlSchema schema in schemas)
        // {
        //     var schemaWalker = new XmlSchemasWalkerService();
        //     var schemaDeclaration = schemaWalker.Walk(schema);
        //     namespaceDeclaration = namespaceDeclaration.AddMembers(schemaDeclaration);
        // }
        // return namespaceDeclaration;
        throw new NotImplementedException();
    }

}