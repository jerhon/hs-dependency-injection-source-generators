using Honlsoft.DependencyInjection.SourceGenerators.Interface;

namespace Honlsoft.DependencyInjection.SourceGenerators.Tests.Interface; 

[Interface]
public partial class SimpleClass {


    [Interface]
    public void Method1() {
        
    }


    [Interface]
    public (int, string) Method2(int parameter1, string parameter2) {
        return (parameter1, parameter2);
    }
}


public class InterfacedClassTest {


    [Fact]
    public void HasCorrectInterface() {
        var iface = typeof(SimpleClass).GetInterfaces().FirstOrDefault((iface) => iface.Name == "ISimpleClass");
        Assert.NotNull(iface);


        var interfacedType = new SimpleClass() as ISimpleClass;
        
        interfacedType.Method1();
        interfacedType.Method2(123, "hello, world!");

    }
}