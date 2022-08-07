using Honlsoft.DependencyInjection.SourceGenerators.FieldConstructor.Domain;
using Microsoft.CodeAnalysis;

namespace Honlsoft.DependencyInjection.SourceGenerators.FieldConstructor; 


[Generator]
public class FactoryConstructorGenerator : ISourceGenerator {
    
    public void Initialize(GeneratorInitializationContext context) {
        context.RegisterForSyntaxNotifications(() => new SyntaxCollector<ClassInfo>(new FactoryConstructorClassInfoMapper()));
    }
    
    public void Execute(GeneratorExecutionContext context) {
        var reciever = context.SyntaxContextReceiver as SyntaxCollector<ClassInfo>;
        var template = new FieldConstructorTemplate();
        
        foreach (var classInfo in reciever.Items)
        {
            context.AddSource($"{classInfo.Namespace}.{classInfo.Name}.g.cs", template.GetClassSource(classInfo));
        }
    }

}