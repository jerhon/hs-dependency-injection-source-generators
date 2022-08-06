using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Honlsoft.DependencyInjection.SourceGenerators.Shared; 

public static class RoslynUtilities {
    
    public static UsingDirectiveSyntax[] GetUsingStatements(this SyntaxNode node) {
        List<UsingDirectiveSyntax> statements = new List<UsingDirectiveSyntax>();
        while (node != null) {
            var usingStatements = node.ChildNodes().OfType<UsingDirectiveSyntax>();
            foreach (var statement in usingStatements) {
                statements.Add(statement);    
            }
            node = node.Parent;
        }
        return statements.ToArray();
    }

    public static bool HasAttribute(this SyntaxList<AttributeListSyntax> listSyntax, string attributeName) {
        var attributes = listSyntax.SelectMany((attrList) => attrList.Attributes.Where((attr) => attr.Name.ToFullString() == attributeName || attr.Name.ToFullString() == attributeName + "Attribute"));
        return attributes.Any();
    }

    public static ClassDeclarationSyntax GetParentClassDeclaration(this SyntaxNode node) {
        while (node.Parent != null) {
            if (node is ClassDeclarationSyntax cds) {
                return cds;
            }
        
            node = node.Parent;
        }

        return null;
    }
    
    private static bool IsMatchingAttribute(AttributeData attributeData, string ns, string name) {
        
        return attributeData?.AttributeClass?.Name == name &&
               attributeData?.AttributeClass?.ContainingNamespace?.ToString() == ns;
    }

    public static bool HasAttribute(this IEnumerable<AttributeData> attributes, string ns, string name) {
        return attributes?.Any((a) => IsMatchingAttribute(a, ns, name)) ?? false;
    }

    
    public static bool HasAttribute(this IEnumerable<AttributeData> attributes, Type type) {
        return attributes?.Any((a) => IsMatchingAttribute(a, type.Namespace, type.Name)) ?? false;
    }
}