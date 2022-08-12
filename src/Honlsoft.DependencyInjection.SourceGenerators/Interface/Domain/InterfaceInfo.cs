namespace Honlsoft.DependencyInjection.SourceGenerators.Interface.Domain;

public class InterfaceInfo {


    public string Namespace { get; set; }
    
    public string[] Usings { get; set; }
    
    public string ClassName { get; set; }
    
    public MethodInfo[] Methods { get; set; }

}