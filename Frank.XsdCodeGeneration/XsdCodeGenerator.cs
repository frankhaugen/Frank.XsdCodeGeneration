using System.Xml.Linq;
using System.Xml.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.XsdCodeGeneration;

public static class XmlSchemaParser
{
    public static XmlSchemaSet ParseXsd(string xsdContent)
    {
        var xsdSchema = XDocument.Parse(xsdContent);
        var schemaSet = new XmlSchemaSet();
        schemaSet.Add("", xsdSchema.CreateReader());
        return schemaSet;
    }
}
public static class ClassFactory
{
    public static ClassDeclarationSyntax GenerateClass(string className, XmlSchemaComplexType complexType)
    {
        var classDeclaration = SyntaxFactory.ClassDeclaration(className)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        var propertyDeclarations = PropertyFactory.GenerateProperties(complexType);

        classDeclaration = classDeclaration.AddMembers(propertyDeclarations.ToArray());

        return classDeclaration;
    }
}
public static class PropertyFactory
    {
        public static List<PropertyDeclarationSyntax> GenerateProperties(XmlSchemaComplexType complexType)
        {
            var propertyDeclarations = new List<PropertyDeclarationSyntax>();

            if (complexType.Particle is XmlSchemaSequence sequence)
            {
                foreach (XmlSchemaElement childElement in sequence.Items)
                {
                    var propertyName = childElement.Name;
                    var propertyType = GetCSharpType(childElement.ElementSchemaType);
                    var propertyDeclaration = GenerateProperty(propertyName, propertyType);
                    propertyDeclarations.Add(propertyDeclaration);
                }
            }

            return propertyDeclarations;
        }

        private static PropertyDeclarationSyntax GenerateProperty(string propertyName, string propertyType)
        {
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(propertyType), propertyName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
                .WithLeadingTrivia(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia)
                            .AddContent(
                                SyntaxFactory.XmlText("/// <summary>"),
                                SyntaxFactory.XmlText($"Gets or sets the {propertyName}."),
                                SyntaxFactory.XmlText("/// </summary>"))));

            return propertyDeclaration;
        }

        private static string GetCSharpType(XmlSchemaType schemaType)
        {
            return schemaType.Name switch
            {
                "string" => "string",
                "int" => "int",
                // Add more type mappings as necessary
                _ => "string"
            };
        }
    }

public class XsdCodeGenerator
{
    public string CreateClasses(XmlSchemaSet schemaSet)
    {
        var classDeclarations = new List<ClassDeclarationSyntax>();

        foreach (XmlSchemaElement element in schemaSet.GlobalElements.Values)
        {
            if (element.ElementSchemaType is XmlSchemaComplexType complexType)
            {
                var classDeclaration = ClassFactory.GenerateClass(element.Name, complexType);
                classDeclarations.Add(classDeclaration);
            }
        }

        var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("GeneratedUBL"))
            .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
            .AddMembers(classDeclarations.ToArray());

        var compilationUnit = SyntaxFactory.CompilationUnit()
            .AddMembers(namespaceDeclaration)
            .NormalizeWhitespace();

        return compilationUnit.ToFullString();
    }

    private IEnumerable<string> ClassesFromSchemaElements(IEnumerable<XmlSchemaElement> elements)
    {
        foreach (var element in elements)
        {
            var classSchema = element.ElementSchemaType as XmlSchemaComplexType;
            if (classSchema != null)
            {
                var properties = CreateAutoPropertiesFromSchema(classSchema);
                var members = new SyntaxList<MemberDeclarationSyntax>(properties);
                var classDeclearationSyntax = SyntaxFactory
                    .ClassDeclaration(classSchema.Name)
                    .WithMembers(members);
                
                classDeclearationSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

                yield return classDeclearationSyntax.NormalizeWhitespace().ToFullString();
            }
        }
    }

    private IEnumerable<PropertyDeclarationSyntax> CreateAutoPropertiesFromSchema(XmlSchemaComplexType schema)
    {
        return schema.Attributes.OfType<XmlSchemaAttribute>()
            .Select(element =>
                SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.ParseTypeName(element.AttributeSchemaType?.Name ?? "ow84u"), element?.Name ?? "srhgw8to8")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddAccessorListAccessors(
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    )
            ).ToArray();
    }

    private ClassDeclarationSyntax CreateClass(string className, XmlSchema schema)
    {
        return SyntaxFactory.ClassDeclaration(className)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddMembers(
                schema.Items.OfType<XmlSchemaElement>()
                    .Select(element =>
                        SyntaxFactory.PropertyDeclaration(
                                SyntaxFactory.ParseTypeName(element.ElementSchemaType.Name), element.Name)
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .AddAccessorListAccessors(
                                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                            )
                    ).ToArray()
            );
    }
}