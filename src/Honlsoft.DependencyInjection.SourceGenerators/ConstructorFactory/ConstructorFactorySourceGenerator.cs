using System;
using System.Reflection;
using Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory.Domain;
using Microsoft.CodeAnalysis;

namespace Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory;

[Generator]
public class ConstructorFactorySourceGenerator : ISourceGenerator {
    
    private static readonly DiagnosticDescriptor GenericError = new DiagnosticDescriptor(id: "HS1000",
        title: "Couldn't autogenerate factories from constructors",
        messageFormat: "Couldn't autogenerate factory {0} {1}",
        category: "Honlsoft.DependencyInjection.SourceGenerators",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    
    public void Initialize(GeneratorInitializationContext context) {
        context.RegisterForSyntaxNotifications(() => new SyntaxCollector<ConstructorFactoryInfo>(new ConstructorFactoryInfoMapper()));
    }
    
    public void Execute(GeneratorExecutionContext context) {

        var receiver = context.SyntaxContextReceiver as SyntaxCollector<ConstructorFactoryInfo>;
        var factoryCodeTemplate = new ConstructorFactoryCodeTemplate();
        
        context.AddSource(typeof(ConstructorFactorySourceGenerator).FullName + ".g.cs", "// V1");
        
        try {
            foreach (var constructorInfo in receiver.Items) {
                context.AddSource($"{constructorInfo.Namespace}.I{constructorInfo.ClassName}Factory.g.cs", factoryCodeTemplate.GetInterfaceSource(constructorInfo));
                context.AddSource($"{constructorInfo.Namespace}.{constructorInfo.ClassName}Factory.g.cs", factoryCodeTemplate.GetImplementationSource(constructorInfo));
            }
        }
        catch (Exception ex) {
            context.ReportDiagnostic(Diagnostic.Create(GenericError, null, ex.ToString(), ex));
        }
    }
}