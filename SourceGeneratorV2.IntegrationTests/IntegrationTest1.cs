using System;
using System.IO;
using System.Reflection;

#if NETCOREAPP1_0_OR_GREATER
using System.Runtime.Loader;
#endif

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

using Xunit;

namespace SourceGeneratorV2.IntegrationTests;

public class IntegrationTest1
{
    [Fact]
    public void Test()
    {
        var generator = new SampleGenerator();

        var parseOptions = CSharpParseOptions.Default
            .WithLanguageVersion(LanguageVersion.Preview)
            .WithDocumentationMode(DocumentationMode.Diagnose);

        var compilerOptions = new CSharpCompilationOptions(
            OutputKind.DynamicallyLinkedLibrary,
            nullableContextOptions: NullableContextOptions.Enable);

        var emitOptions = new EmitOptions(
            debugInformationFormat: DebugInformationFormat.PortablePdb);

        var driver = CSharpGeneratorDriver.Create(
            new[] { generator.AsSourceGenerator() },
            parseOptions: parseOptions);

        var asmName = Guid.NewGuid().ToString();

        var compilation = CSharpCompilation.Create(
            asmName,
            options: compilerOptions);

        var updatedDriver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out var updatedCompilation,
            out var diagnostics);

        var result = updatedDriver.GetRunResult();

        using var peStream = new MemoryStream();
        using var pdbStream = new MemoryStream();

        var emitResult = updatedCompilation.Emit(
            peStream,
            pdbStream,
            options: emitOptions);

        if (!emitResult.Success)
        {
            return;
        }
        
        peStream.Seek(0, SeekOrigin.Begin);
        pdbStream.Seek(0, SeekOrigin.Begin);

        Assembly assembly;

#if NETCOREAPP1_0_OR_GREATER
        assembly = AssemblyLoadContext.Default.LoadFromStream(peStream, pdbStream);
#else
        assembly = Assembly.Load(peStream.GetBuffer(), pdbStream.GetBuffer());
#endif
    }
}
