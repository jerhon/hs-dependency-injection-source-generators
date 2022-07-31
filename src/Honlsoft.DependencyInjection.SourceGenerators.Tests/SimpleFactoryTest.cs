using Microsoft.Extensions.DependencyInjection;

namespace Honlsoft.DependencyInjection.SourceGenerators.Tests;

public class SimpleFactoryTest {
    [SetUp]
    public void Setup() {
    }

    [Test]
    public void TestExecutionOfSimpleFactory() 
    {
        IInstanceClassFactory factory = new InstanceClassFactory(new InjectedClass());
        var testClass = factory.Create("Factory");
        
        Assert.AreEqual("Factory Injected!", testClass.ToString());

        //asdf
    }
}

// TODO: Need to copy over all namespace references


public class InjectedClass {
    public override string ToString() {
        return "Injected!";
    }
}

public class InstanceClass {

    private readonly string _regularParameter;
    private readonly InjectedClass _injectedParameter;
    
    [Factory]
    public InstanceClass(string regularParameter, [Inject]InjectedClass injectedClass) {
        _regularParameter = regularParameter;
        _injectedParameter = injectedClass;
    }

    public override string ToString() {
        return $"{_regularParameter} {_injectedParameter}";
    }
}
