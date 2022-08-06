using System;
using Microsoft.CodeAnalysis;


namespace Honlsoft.DependencyInjection.SourceGenerators;

[Generator]
public class FactoryGenerator : ISourceGenerator {
    
    private static readonly DiagnosticDescriptor GenericError = new DiagnosticDescriptor(id: "HSDISG0000",
        title: "Couldn't autogenerate factory from constructor",
        messageFormat: "Couldn't autogenerate factory {0} {1}",
        category: "Honlsoft.DependencyInjection.SourceGenerators",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    public void Initialize(GeneratorInitializationContext context) {
        context.RegisterForSyntaxNotifications(() => new FactorySyntaxReceiver());
    }
    
    public void Execute(GeneratorExecutionContext context) {

        var receiver = context.SyntaxContextReceiver as FactorySyntaxReceiver;
        var factoryCodeTemplate = new FactoryCodeTemplate();
        
        context.AddSource(typeof(FactoryGenerator).FullName + ".g.cs", "// V1");
        
        try {
            foreach (var constructorInfo in receiver.Result) {
                context.AddSource($"{constructorInfo.Namespace}.I{constructorInfo.ClassName}Factory.g.cs", factoryCodeTemplate.GetInterfaceSource(constructorInfo));
                context.AddSource($"{constructorInfo.Namespace}.{constructorInfo.ClassName}Factory.g.cs", factoryCodeTemplate.GetImplementationSource(constructorInfo));
            }
        }
        catch (Exception ex) {
            context.ReportDiagnostic(Diagnostic.Create(GenericError, null, ex.ToString(), ex));
        }
    }
}