using Frank.XsdCodeGeneration;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Xunit.Abstractions;

namespace Frank.XsdCodeGeneration.Tests;

[TestSubject(typeof(XmlSchemaSetWalkerService))]
public class XsdWalkerServiceTests
{
    private readonly ITestOutputHelper _output;

    public XsdWalkerServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Walk()
    {
        var xsdFile = new FileInfo(Path.Combine(AppContext.BaseDirectory, "xsd", "ubl", "maindoc", "UBL-Invoice-2.1.xsd"));
        var xsd = XmlSchemaSetLoader.Load(xsdFile);
        var walker = new XmlSchemaSetWalkerService();
        var result = walker.Walk(xsd);
        _output.WriteLine(result.NormalizeWhitespace().ToFullString());
    }
}