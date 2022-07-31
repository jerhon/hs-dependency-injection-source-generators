using System.Collections.Concurrent;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Honlsoft.DependencyInjection.SourceGenerators; 

/// <summary>
/// Collects syntax nodes responsible for creating an injectable factory.
/// </summary>
public class FactorySyntaxReceiver : ISyntaxReceiver {
    
    // TODO: Think about experimenting with getting information of the semantic model as well in the Execute method on the Source Generator

    public ConcurrentBag<FactoryConstructorInfo> ConstructorInfo { get; }
    
    public FactorySyntaxReceiver() {
        ConstructorInfo = new ConcurrentBag<FactoryConstructorInfo>();
    }
    
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode) {
        if (syntaxNode is ConstructorDeclarationSyntax constructor && HasFactoryAttribute(constructor)) {
            
            // For each parameter, look for an attribute that matches the "Inject"
            var parameter = constructor.ParameterList.Parameters.Select((p) =>
                new FactoryParameterInfo{
                    Name = p.Identifier.Text,
                    Type = p.Type?.GetText()?.ToString(),
                    Injected = HasInjectAttribute(p),
                });
            
            this.ConstructorInfo.Add(new FactoryConstructorInfo {
                ClassName = constructor.Identifier.Text, 
                Parameters = parameter.ToArray(),
                Namespace = GetNamespace(constructor),
            });

        }
    }

    private bool HasAttribute(SyntaxList<AttributeListSyntax> listSyntax, string attributeName) {
        
        // TODO: Need to determine a way to double check the namespace of the attribute?
        
        var attributes = listSyntax.SelectMany((attrList) => attrList.Attributes.Where((attr) => attr.Name.ToFullString() == attributeName || attr.Name.ToFullString() == attributeName + "Attribute"));
        return attributes.Any();
    }

    private bool HasInjectAttribute(ParameterSyntax parameter) {
        return HasAttribute(parameter.AttributeLists, "Inject");
    }
    
    private bool HasFactoryAttribute(ConstructorDeclarationSyntax constructor) {
        return HasAttribute(constructor.AttributeLists, "Factory");
    }

    private string GetNamespace(SyntaxNode node) {
        // Need to be much smarter about finding this
        //
        // Need to deal with situations such as sub-classes, nested namespaces, etc.
        
        while (node.Parent != null) {
            if (node.Parent is NamespaceDeclarationSyntax ns) {
                return ns.Name.ToString();
            }
            node = node.Parent;
        }
        
        // Find the root node
        var fileNamespace = node.DescendantNodes((node) => true).OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
        return fileNamespace?.Name.ToString() ?? string.Join(", ", node.ChildNodes().Select((cn) => cn.GetType().ToString()));
    }
}