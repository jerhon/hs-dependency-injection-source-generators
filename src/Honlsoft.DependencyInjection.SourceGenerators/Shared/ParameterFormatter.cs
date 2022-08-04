using System.Collections.Generic;
using System.Linq;

namespace Honlsoft.DependencyInjection.SourceGenerators.Shared; 

public class ParameterFormatter {
    
    public string FormatParameterList(IEnumerable<IParameter> parameters) {
        return string.Join(",", parameters.Select((p) => $"{p.Type} {p.Name}"));
    }
    
    
}