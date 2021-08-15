using Microsoft.CodeAnalysis;

namespace SourceGeneratorV2;

[Generator]
public class SampleGenerator :
    IIncrementalGenerator
{
    void IIncrementalGenerator.Initialize(
        IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(this.OnPostInitialize);
    }

    private void OnPostInitialize(
        IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource("Foo.cs", "// foo");
    }
}
