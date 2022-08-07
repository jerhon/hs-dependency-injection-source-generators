using Microsoft.CodeAnalysis;

namespace Honlsoft.DependencyInjection.SourceGenerators; 

public interface ISyntaxMapper<TResult> {

    TResult MapSyntax(SyntaxNode syntaxNode, SemanticModel semanticModel);

}