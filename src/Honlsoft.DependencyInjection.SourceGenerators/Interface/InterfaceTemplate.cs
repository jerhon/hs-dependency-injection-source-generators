using System.Text;
using Honlsoft.DependencyInjection.SourceGenerators.Interface.Domain;

namespace Honlsoft.DependencyInjection.SourceGenerators.Interface; 

public class InterfaceTemplate {

    public string GenerateInterfaceTemplate(InterfaceInfo info) {
        StringBuilder sb = new StringBuilder();
        
        foreach (var use in info.Usings) {
            sb.AppendLine(use);
        }
        sb.AppendLine();
        sb.AppendLine($"namespace {info.Namespace};");
        sb.AppendLine();
        sb.AppendLine($"public interface I{info.ClassName} {{");
        foreach (var method in info.Methods) {
            sb.AppendLine(GenerateMethodTemplate(method));
        }
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine($"public partial class {info.ClassName} : I{info.ClassName} {{}}");
        sb.AppendLine();
        return sb.ToString();
    }

    public string GenerateMethodTemplate(MethodInfo info) {
        return $"   {info.Return} {info.Name}{info.ParameterList};";
    }
}