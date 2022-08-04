using Honlsoft.DependencyInjection.SourceGenerators.Shared;

namespace Honlsoft.DependencyInjection.SourceGenerators.Constructor.Domain; 

public class FieldInfo : IParameter {

    /// <summary>
    /// The type of the field.
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The name of the field.
    /// </summary>
    public string Name { get; set; } 
}