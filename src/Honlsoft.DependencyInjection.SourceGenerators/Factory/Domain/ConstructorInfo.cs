using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Honlsoft.DependencyInjection.SourceGenerators; 

public class FactoryConstructorInfo {
    
    public string Namespace { get; set; }
    
    public string ClassName { get; set; }
    
    public string[] Usings { get; set; }

    public FactoryParameterInfo[] Parameters { get; set; }
    
    

}