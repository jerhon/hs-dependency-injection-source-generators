using System.Text;
using Honlsoft.DependencyInjection.SourceGenerators.Constructor.Domain;
using Honlsoft.DependencyInjection.SourceGenerators.Shared;

namespace Honlsoft.DependencyInjection.SourceGenerators.Constructor.Templates; 

public class ConstructorTemplate {
    
    public string GetClassSource(ClassInfo classInfo) {

        StringBuilder builder = new StringBuilder();
        
        builder.AppendLine($"namespace {classInfo.Namespace};");
        builder.AppendLine();
        foreach (var usingStatement in classInfo.Usings) {
            builder.AppendLine(usingStatement);
        }
        builder.AppendLine();
        
        // TODO: Modifiers?
        builder.AppendLine($"public partial class {classInfo.Name} {{");
        builder.AppendLine(GetConstructorImplementation(classInfo));
        builder.AppendLine($"}}");
        return builder.ToString();
    }

    public string GetConstructorImplementation(ClassInfo classInfo) {
        StringBuilder builder = new StringBuilder();
        ParameterFormatter formatter = new ParameterFormatter();
        string parameters = formatter.FormatParameterList(classInfo.Fields);
        builder.AppendLine($"   public {classInfo.Name}({parameters}) {{");
        foreach (var parameter in classInfo.Fields) {
            builder.AppendLine($"       this.{parameter.Name} = {parameter.Name};");
        }
        builder.AppendLine($"   }}");
        return builder.ToString();
    }
}