using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;

namespace Honlsoft.DependencyInjection.SourceGenerators; 

public class SyntaxCollector<TType> : ISyntaxContextReceiver {

    public ConcurrentQueue<TType> Items { get; } = new ConcurrentQueue<TType>();

    private readonly ISyntaxMapper<TType> _syntaxMapper;

    public SyntaxCollector(ISyntaxMapper<TType> syntaxMapper) {
        this._syntaxMapper = syntaxMapper;
    }

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {
        var syntax = _syntaxMapper.MapSyntax(context.Node, context.SemanticModel);
        if (syntax != null) {
            Items.Enqueue(syntax);
        }
    }
}