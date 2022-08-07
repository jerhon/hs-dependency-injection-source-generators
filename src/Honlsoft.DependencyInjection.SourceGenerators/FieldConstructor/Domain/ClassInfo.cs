namespace Honlsoft.DependencyInjection.SourceGenerators.FieldConstructor.Domain; 

public class ClassInfo {

    /// <summary>
    /// The namespace of the class.
    /// </summary>
    public string Namespace { get; set; }
    
    /// <summary>
    /// The name of the class.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Using statements from the original file.
    /// </summary>
    public string[] Usings { get; set; }

    /// <summary>
    /// Fields that need to be initialized.
    /// </summary>
    public FieldInfo[] Fields { get; set; }
}