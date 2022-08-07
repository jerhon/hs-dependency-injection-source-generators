using Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory;

namespace Honlsoft.DependencyInjection.SourceGenerators.Tests.Factory;

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
