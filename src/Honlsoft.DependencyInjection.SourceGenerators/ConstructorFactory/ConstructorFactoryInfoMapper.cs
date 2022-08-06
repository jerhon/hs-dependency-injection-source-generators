using System.Linq;
using Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory.Domain;
using Honlsoft.DependencyInjection.SourceGenerators.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Honlsoft.DependencyInjection.SourceGenerators.ConstructorFactory; 

/// <summary>
/// Maps the roslyn syntax and semantic models to the internal types used to generate the constructor info.
/// </summary>
public class ConstructorFactoryInfoMapper {

    public FactoryConstructorInfo MapConstructorInfo(SyntaxNode syntaxNode, SemanticModel semanticModel) {
        
        // TODO: Add validation
        // * Don't support nested namespaces / usings, throw an error for those
        
        if (syntaxNode is ConstructorDeclarationSyntax constructor && HasFactoryAttribute(constructor)) {
            
            var constructorSymbol = semanticModel.GetDeclaredSymbol(constructor) ;
            if (constructorSymbol == null || !constructorSymbol.GetAttributes().HasAttribute(typeof(FactoryAttribute))) {
                return null;
            }

            var classDeclaration = constructor.GetParentClassDeclaration();
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            if (classSymbol == null) {
                return null;
            }
            
            // For each parameter, look for an attribute that matches the "Inject"
            // This will get us to an an initial look, we will narrow it down below
            var parameters = constructor.ParameterList.Parameters.Select((p) =>  MapParameterInfo(semanticModel, p));
            
            return new FactoryConstructorInfo {
                ClassName = constructor.Identifier.Text, 
                Parameters = parameters.ToArray(),
                Namespace = classSymbol.ContainingNamespace.ToString(),
                Usings = classDeclaration.GetUsingStatements().Select((u) => u.ToString()).ToArray()
            };
        }

        return null;
    }


    private FactoryParameterInfo MapParameterInfo(SemanticModel semanticModel, ParameterSyntax parameter) {

        var parameterSymbol = semanticModel.GetDeclaredSymbol(parameter) as IParameterSymbol;

        return new FactoryParameterInfo {
            Name = parameter.Identifier.Text,
            Type = parameter.Type?.GetText()?.ToString(),
            Injected = parameterSymbol?.GetAttributes().HasAttribute(typeof(InjectAttribute)) ?? false
        };
    }

    private (string, string) GetParameterType(SemanticModel model, IParameterSymbol parameterSymbol) {

        return (parameterSymbol.Type?.OriginalDefinition?.ContainingNamespace?.ToString() ?? parameterSymbol?.Type?.ContainingNamespace.ToString(), "");
    }
    
    private bool HasInjectAttribute(ParameterSyntax parameter) {
        return parameter.AttributeLists.HasAttribute("Inject");
    }
    
    private bool HasFactoryAttribute(ConstructorDeclarationSyntax constructor) {
        return constructor.AttributeLists.HasAttribute("Factory");
    }

}