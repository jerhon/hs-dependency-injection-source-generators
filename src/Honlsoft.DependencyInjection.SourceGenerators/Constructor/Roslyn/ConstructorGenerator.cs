using System.Reflection.Metadata;
using System.Text;
using Honlsoft.DependencyInjection.SourceGenerators.Constructor.Domain;
using Honlsoft.DependencyInjection.SourceGenerators.Shared;
using Microsoft.CodeAnalysis;

namespace Honlsoft.DependencyInjection.SourceGenerators.Constructor.Roslyn; 


[Generator]
public class ConstructorGenerator : ISourceGenerator {
    
    
    public void Initialize(GeneratorInitializationContext context) {
        context.RegisterForSyntaxNotifications(() => new ConstructorSyntaxReceiver());
    }
    public void Execute(GeneratorExecutionContext context) {
        var reciever = context.SyntaxContextReceiver as ConstructorSyntaxReceiver;
        var template = new ConstructorTemplate();
        
        foreach (var classInfo in reciever.ClassInfos)
        {
            context.AddSource($"{classInfo.Namespace}.{classInfo.Name}.g.cs", template.GetClassSource(classInfo));
        }
    }

}