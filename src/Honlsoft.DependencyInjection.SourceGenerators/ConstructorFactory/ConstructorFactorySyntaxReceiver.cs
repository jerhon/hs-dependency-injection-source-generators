using System.Collections.Concurrent;
using Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory.Domain;
using Microsoft.CodeAnalysis;

namespace Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory; 

/// <summary>
/// Collects syntax nodes responsible for creating an injectable factory.
/// </summary>
public class ConstructorFactorySyntaxReceiver : ISyntaxContextReceiver {

    public ConcurrentBag<FactoryConstructorInfo> Result { get; }
    
    public ConstructorFactorySyntaxReceiver() {
        Result = new ConcurrentBag<FactoryConstructorInfo>();
    }

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {

        ConstructorFactoryInfoMapper mapper = new ConstructorFactoryInfoMapper();
        var constructorInfo = mapper.MapConstructorInfo(context.Node, context.SemanticModel);
        if (constructorInfo != null) {
            Result.Add(constructorInfo);
        }
    }
}