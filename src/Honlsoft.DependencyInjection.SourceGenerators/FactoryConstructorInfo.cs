namespace Honlsoft.DependencyInjection.SourceGenerators; 

public class FactoryConstructorInfo {
    
    public string Namespace { get; set; }
    public string ClassName { get; set; }

    public FactoryParameterInfo[] Parameters { get; set; }
    
}