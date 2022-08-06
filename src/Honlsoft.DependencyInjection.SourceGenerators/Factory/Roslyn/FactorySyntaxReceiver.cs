using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;

namespace Honlsoft.DependencyInjection.SourceGenerators; 

/// <summary>
/// Collects syntax nodes responsible for creating an injectable factory.
/// </summary>
public class FactorySyntaxReceiver : ISyntaxContextReceiver {

    public ConcurrentBag<FactoryConstructorInfo> Result { get; }
    
    public FactorySyntaxReceiver() {
        Result = new ConcurrentBag<FactoryConstructorInfo>();
    }

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {

        ConstructorInfoMapper mapper = new ConstructorInfoMapper();
        var constructorInfo = mapper.MapConstructorInfo(context.Node, context.SemanticModel);
        if (constructorInfo != null) {
            Result.Add(constructorInfo);
        }
    }
}