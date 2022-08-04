using System.Collections.Concurrent;
using System.Linq;
using Honlsoft.DependencyInjection.SourceGenerators.Constructor.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Honlsoft.DependencyInjection.SourceGenerators.Constructor.Roslyn; 

public class ConstructorSyntaxReceiver : ISyntaxContextReceiver {
    
    public ConcurrentBag<ClassInfo> ClassInfos { get; } = new ConcurrentBag<ClassInfo>();
    
    public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {

        // Find all classes with fields with an [Inject] attribute
        if (context.Node is ClassDeclarationSyntax classDeclaration) {

            var fields = classDeclaration.Members.OfType<FieldDeclarationSyntax>()
                .Where(HasInjectAttribute)
                .Select(MapToFieldInfo)
                .ToArray();

            if (fields.Any()) {
                var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
                
                var classInfo = new ClassInfo() {
                    Usings = classDeclaration.GetUsingStatements().Select((us) => us.ToFullString()).ToArray(),
                    Namespace = classSymbol?.ContainingNamespace.ToString(),
                    Fields = fields.ToArray(),
                    Name = classDeclaration.Identifier.ToString()
                };
                
                ClassInfos.Add(classInfo);
            }
        }
    }

    private bool HasInjectAttribute(FieldDeclarationSyntax fieldDeclaration) {
        return fieldDeclaration.AttributeLists.HasAttribute("Inject");
    }

    private FieldInfo MapToFieldInfo(FieldDeclarationSyntax fieldDeclaration) {
        return new FieldInfo {
            Name = fieldDeclaration.Declaration.Variables.First().Identifier.ToString(),
            Type = fieldDeclaration.Declaration.Type.ToFullString(),
        };
    }
}