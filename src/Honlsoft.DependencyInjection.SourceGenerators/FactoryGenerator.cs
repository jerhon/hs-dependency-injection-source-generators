using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;


namespace Honlsoft.DependencyInjection.SourceGenerators;



[Generator]
public class FactoryGenerator : ISourceGenerator {
    
    // TODO: Need to copy over namespace references for used parameters?
    // TODO: Better handling of namespaces, (nested classes, nested namespaces, etc).

    
    private static readonly DiagnosticDescriptor GenericError = new DiagnosticDescriptor(id: "HSDISG0000",
        title: "Couldn't autogenerate factory",
        messageFormat: "Couldn't autogenerate factory {0}",
        category: "Honlsoft.DependencyInjection.SourceGenerators",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    public void Initialize(GeneratorInitializationContext context) {
        context.RegisterForSyntaxNotifications(() => new FactorySyntaxReceiver());
    }
    
    public void Execute(GeneratorExecutionContext context) {

        var reciever = context.SyntaxReceiver as FactorySyntaxReceiver;
        
        try {

            // Just some debug info...
            string source = string.Join("\n", reciever.ConstructorInfo.Select(SourcifyConstructor));
            context.AddSource("Generated1.cs", source);


            foreach (var constructorInfo in reciever.ConstructorInfo) {
                context.AddSource($"I{constructorInfo.ClassName}Factory.cs", SourcifyFactoryInterface(constructorInfo));
                context.AddSource($"{constructorInfo.ClassName}Factory.cs", SourcifyFactoryImplementation(constructorInfo));
            }
        }
        catch (Exception ex) {
            context.ReportDiagnostic(Diagnostic.Create(GenericError, null, ex));
        }

    }

    private string SourcifyConstructor(FactoryConstructorInfo ci) {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"// {ci.ClassName}  {ci.Namespace}");
        foreach (var parameter in ci.Parameters) {
            sb.AppendLine($"//    {parameter.Type} {parameter.Name} - Injected? {parameter.Injected}");
        }
        return sb.ToString();
    }

    private string SourcifyFactoryInterface(FactoryConstructorInfo ci) {

        var parameterList = SourcifyParameterLists( ci.Parameters.Where((p) => !p.Injected));
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"namespace {ci.Namespace};");
        sb.AppendLine($"public interface I{ci.ClassName}Factory {{");
        sb.AppendLine($"\t{ci.ClassName} Create({parameterList});");
        sb.AppendLine($"}}\n\n");
        return sb.ToString();
    }

    private string SourcifyFactoryImplementation(FactoryConstructorInfo ci) {
        
        var injectableParameterList = SourcifyParameterLists( ci.Parameters.Where((p) => p.Injected));
        var nonInjectableParameterList = SourcifyParameterLists( ci.Parameters.Where((p) => !p.Injected));
        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"namespace {ci.Namespace};");
        sb.AppendLine($"public class {ci.ClassName}Factory : I{ci.ClassName}Factory {{");

        sb.AppendLine("");
        foreach (var parameter in ci.Parameters) {
            if (parameter.Injected) {
                sb.AppendLine($"    private readonly {parameter.Type} _{parameter.Name};");
            }
        }
        sb.AppendLine();
        sb.AppendLine($"    public {ci.ClassName}Factory({injectableParameterList}) {{");
        foreach (var parameter in ci.Parameters) {
            if (parameter.Injected) {
                sb.AppendLine($"        _{parameter.Name} = {parameter.Name};");
            }
        }
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine($"    public {ci.ClassName} Create({nonInjectableParameterList}) {{");
        
        sb.Append("        return new(");
        foreach (var parameter in ci.Parameters) {

            if (parameter.Injected) {
                sb.Append("_");
            }
            sb.Append(parameter.Name);
            sb.Append(", ");
        }
        if (ci.Parameters.Any()) {
            sb.Length = sb.Length - 2;
        }

        sb.AppendLine(");");
        
        sb.AppendLine("    }");
        sb.AppendLine("}\n");
        return sb.ToString();
        
    }

    private string SourcifyParameterLists(IEnumerable<FactoryParameterInfo> parameters) {
        return string.Join(", ", parameters.Select((p) => $"{p.Type} {p.Name}"));
    }

    
}