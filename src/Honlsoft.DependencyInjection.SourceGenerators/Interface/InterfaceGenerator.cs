using Honlsoft.DependencyInjection.SourceGenerators.Interface.Domain;
using Microsoft.CodeAnalysis;

namespace Honlsoft.DependencyInjection.SourceGenerators.Interface; 

[Generator]
public class InterfaceGenerator : ISourceGenerator {

    public void Initialize(GeneratorInitializationContext context) {
        context.RegisterForSyntaxNotifications(() => new SyntaxCollector<InterfaceInfo>(new InterfaceInfoMapper()));
    }
    public void Execute(GeneratorExecutionContext context) {
        var syntaxReciever = context.SyntaxContextReceiver as SyntaxCollector<InterfaceInfo>;
        var template = new InterfaceTemplate();
        foreach (var item in syntaxReciever.Items) {
            var interfaceSource = template.GenerateInterfaceTemplate(item);
            context.AddSource($"{item.Namespace}.{item.ClassName}.g.cs", interfaceSource);
        }
    }
}