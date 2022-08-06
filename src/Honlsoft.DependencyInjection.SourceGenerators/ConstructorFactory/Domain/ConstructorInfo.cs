namespace Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory.Domain; 

public class FactoryConstructorInfo {
    
    public string Namespace { get; set; }
    
    public string ClassName { get; set; }
    
    public string[] Usings { get; set; }

    public FactoryParameterInfo[] Parameters { get; set; }
    
    

}