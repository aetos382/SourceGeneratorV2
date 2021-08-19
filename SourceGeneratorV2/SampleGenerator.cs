using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGeneratorV2;

[Generator]
public sealed class SampleGenerator :
    IIncrementalGenerator
{
    void IIncrementalGenerator.Initialize(
        IncrementalGeneratorInitializationContext context)
    {
    }
}
