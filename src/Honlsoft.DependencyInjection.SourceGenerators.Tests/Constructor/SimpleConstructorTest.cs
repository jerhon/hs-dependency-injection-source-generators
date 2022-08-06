using Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory;
using Xunit;

namespace Honlsoft.DependencyInjection.SourceGenerators.Tests.Constructor; 


public partial class Constructed {

    [Inject] private readonly string _parameter1;

    [Inject] private readonly string _parameter2;
    
    public override string ToString() {
        return $"{_parameter1} {_parameter2}";
    }
}


public class SimpleConstructorTest {

    [Fact]
    public void SimpleConstructorSuccess() {
        var constructed = new Constructed("1", "2");
        Assert.Equal("1 2", constructed.ToString());
    }
    
}
