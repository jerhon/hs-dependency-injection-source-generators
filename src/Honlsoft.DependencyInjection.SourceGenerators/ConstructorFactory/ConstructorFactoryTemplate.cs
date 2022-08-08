using System.Collections.Generic;
using System.Linq;
using System.Text;
using Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory.Domain;

namespace Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory; 


/// <summary>
/// Generates the source code for the Constructor Factory from a template. 
/// </summary>
public class ConstructorFactoryCodeTemplate {
    
    public string GetInterfaceSource(ConstructorFactoryInfo ci) {

        var parameterList = SourcifyParameterLists( ci.Parameters.Where((p) => !p.Injected));
        StringBuilder sb = new StringBuilder();
        
        foreach (var usingStatement in ci.Usings) {
            sb.AppendLine(usingStatement);
        }
        
        sb.AppendLine();

        
        sb.AppendLine($"namespace {ci.Namespace};");

        sb.AppendLine();
        
        sb.AppendLine($"public interface I{ci.ClassName}Factory {{");
        sb.AppendLine($"\t{ci.ClassName} Create({parameterList});");
        sb.AppendLine($"}}\n\n");
        return sb.ToString();
    }

    public string GetImplementationSource(ConstructorFactoryInfo ci) {
        
        var injectableParameterList = SourcifyParameterLists( ci.Parameters.Where((p) => p.Injected));
        var nonInjectableParameterList = SourcifyParameterLists( ci.Parameters.Where((p) => !p.Injected));
        
        StringBuilder sb = new StringBuilder();
        foreach (var usingStatement in ci.Usings) {
            sb.AppendLine(usingStatement);
        }
        sb.AppendLine();
        sb.AppendLine($"namespace {ci.Namespace};");
        sb.AppendLine();
        sb.AppendLine($"public class {ci.ClassName}Factory : I{ci.ClassName}Factory {{");

        sb.AppendLine("");
        foreach (var parameter in ci.Parameters) {
            if (parameter.Injected) {
                sb.AppendLine($"    private readonly {GetFullyTypedName(parameter.TypeNamespace, parameter.Type)} _{parameter.Name};");
            }
        }
        sb.AppendLine();
        sb.AppendLine($"    public {ci.ClassName}Factory({injectableParameterList}) {{");
        foreach (var parameter in ci.Parameters) {
            if (parameter.Injected) {
                sb.AppendLine($"        this._{parameter.Name} = {parameter.Name};");
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

    private string SourcifyParameterLists(IEnumerable<ConstructorFactoryParameterInfo> parameters) {
        return string.Join(", ", parameters.Select((p) => $"{GetFullyTypedName(p.TypeNamespace, p.Type)} {p.Name}"));
    }


    public string GetFullyTypedName(string ns, string type) {
        if (!string.IsNullOrWhiteSpace(ns)) {
            return ns + "." + type;
        }

        return type;
    }

}