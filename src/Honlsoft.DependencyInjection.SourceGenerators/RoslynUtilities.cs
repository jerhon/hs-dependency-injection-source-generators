using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Honlsoft.DependencyInjection.SourceGenerators; 

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
}