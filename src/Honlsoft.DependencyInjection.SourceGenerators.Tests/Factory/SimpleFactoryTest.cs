using Honlsoft.DependencyInjection.SourceGenerators.Tests.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Honlsoft.DependencyInjection.SourceGenerators.Tests;

public class SimpleFactoryTest {

    [Fact]
    public void TestExecutionOfSimpleFactory() 
    {
        IInstanceClassFactory factory = new InstanceClassFactory(new InjectedClass());
        var testClass = factory.Create("Factory", "Another", "Final");
        
        Assert.Equal("Factory Injected! Another Final", testClass.ToString());
    }
}

public class InstanceClass {
    private readonly string _verificationString;
    
    [Factory]
    public InstanceClass(string regularParameter, [Inject]InjectedClass injectedClass, string anotherParameter, string finalParameter) {
        this._verificationString = $"{regularParameter} {injectedClass} {anotherParameter} {finalParameter}";
    }

    public override string ToString() {
        return _verificationString;
    }
}
