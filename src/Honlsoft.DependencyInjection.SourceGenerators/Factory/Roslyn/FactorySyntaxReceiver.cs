using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Honlsoft.DependencyInjection.SourceGenerators; 

/// <summary>
/// Collects syntax nodes responsible for creating an injectable factory.
/// </summary>
public class FactorySyntaxReceiver : ISyntaxContextReceiver {

    public ConcurrentBag<FactoryConstructorInfo> ConstructorInfo { get; }
    
    public FactorySyntaxReceiver() {
        ConstructorInfo = new ConcurrentBag<FactoryConstructorInfo>();
    }
    
    private bool HasInjectAttribute(ParameterSyntax parameter) {
        return parameter.AttributeLists.HasAttribute("Inject");
    }
    
    private bool HasFactoryAttribute(ConstructorDeclarationSyntax constructor) {
        return constructor.AttributeLists.HasAttribute("Factory");
    }

    private bool IsMatchingAttribute(AttributeData attributeData, string name) {
        
        return attributeData?.AttributeClass?.Name == name &&
               attributeData?.AttributeClass?.ContainingNamespace?.ToString() == "Honlsoft.DependencyInjection.SourceGenerators";
    }

    private bool HasAttribute(IEnumerable<AttributeData> attributes, string name) {
        return attributes?.Any((a) => IsMatchingAttribute(a, name)) ?? false;
    }

    private ClassDeclarationSyntax GetClassDeclaration(SyntaxNode node) {
        while (node.Parent != null) {
            if (node is ClassDeclarationSyntax cds) {
                return cds;
            }
            
            node = node.Parent;
        }

        return null;
    }

    public (string, string) GetParameterType(SemanticModel model, IParameterSymbol parameterSymbol) {

        return (parameterSymbol.Type?.OriginalDefinition?.ContainingNamespace?.ToString() ?? parameterSymbol?.Type?.ContainingNamespace.ToString(), "");
    }


    public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {
        
        // TODO: Add validation
        // * Don't support nested namespaces / usings, throw an error for those

        var syntaxNode = context.Node;

        if (syntaxNode is ConstructorDeclarationSyntax constructor && HasFactoryAttribute(constructor)) {
            
            // For each parameter, look for an attribute that matches the "Inject"
            // This will get us to an an initial look, we will narrow it down below
            var parameter = constructor.ParameterList.Parameters.Select((p) =>
                new FactoryParameterInfo{
                    Name = p.Identifier.Text,
                    Type = p.Type?.GetText()?.ToString(),
                    Injected = HasInjectAttribute(p),
                    
                });


            var classDeclaration = GetClassDeclaration(constructor);
            var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
            if (classSymbol == null) {
                return;
            }


            // Double check for the proper inject attribute
            var constructorSymbol = context.SemanticModel.GetDeclaredSymbol(constructor);
            var attributes = constructorSymbol?.GetAttributes();
            if (HasAttribute(attributes, "Factory")) 
            {
                return;
            }
            
            constructorSymbol?.Parameters.Select((p) => {
                var (name, ns) = GetParameterType(context.SemanticModel, p); 
                
                return new FactoryParameterInfo() {
                    Name = p.Name,
                    Type = name,
                    TypeNamespace = ns,
                    Injected = HasAttribute(p.GetAttributes(), "Inject")
                };
            });
            
            
            this.ConstructorInfo.Add(new FactoryConstructorInfo {
                ClassName = constructor.Identifier.Text, 
                Parameters = parameter.ToArray(),
                Namespace = classSymbol.ContainingNamespace.ToString(),
                Usings = classDeclaration.GetUsingStatements().Select((u) => u.ToString()).ToArray()
            });

        }
    }
}