using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Frank.XsdCodeGeneration.Tests;

[TestSubject(typeof(XmlSchemaSetLoader))]
public class XsdLoaderTest
{
    private readonly ITestOutputHelper _output;

    public XsdLoaderTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void LoadAndCompile()
    {
        var xsdFile = new FileInfo(Path.Combine(AppContext.BaseDirectory, "xsd", "ubl", "maindoc", "UBL-Invoice-2.1.xsd"));
        var xsd = XmlSchemaSetLoader.Load(xsdFile);
        
        xsd.Compile();
        
        _output.WriteLine(xsd.IsCompiled.ToString());
    }
}