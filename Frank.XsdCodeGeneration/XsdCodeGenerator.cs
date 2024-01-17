using System.Xml.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Frank.XsdCodeGeneration;

public class XsdCodeGenerator
{
    private void CreateClasses(XmlSchemaSet schemaSet)
    {
        var schemas = schemaSet.ToSchemas();
        var elements = schemas.OfType<XmlSchema>().SelectMany(schema => schema.Items.OfType<XmlSchemaElement>());
        ClassesFromSchemaElements(elements);
    }

    private void ClassesFromSchemaElements(IEnumerable<XmlSchemaElement> elements)
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

                classDeclearationSyntax.NormalizeWhitespace().ToFullString().Dump();
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