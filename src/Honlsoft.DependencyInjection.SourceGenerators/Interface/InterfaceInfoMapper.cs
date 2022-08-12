using System.Linq;
using Honlsoft.DependencyInjection.SourceGenerators.FieldConstructor.Domain;
using Honlsoft.DependencyInjection.SourceGenerators.Interface.Domain;
using Honlsoft.DependencyInjection.SourceGenerators.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Honlsoft.DependencyInjection.SourceGenerators.Interface; 

public class InterfaceInfoMapper : ISyntaxMapper<InterfaceInfo> {



    public InterfaceInfo MapSyntax(SyntaxNode syntaxNode, SemanticModel semanticModel) {

        if (syntaxNode is ClassDeclarationSyntax cds) {

            var classSymbol = semanticModel.GetDeclaredSymbol(cds);
            if (classSymbol == null)
                return null;

            if (!classSymbol.GetAttributes().HasAttribute(typeof(InterfaceAttribute)))
                return null;

            var methods = syntaxNode.DescendantNodesAndSelf()
                .OfType<MethodDeclarationSyntax>()
                .Select((mds) => MapMethod(mds, semanticModel))
                .Where((mds) => mds != null);

            var info = new InterfaceInfo() {
                ClassName = cds.Identifier.ToString(),
                Namespace = classSymbol.ContainingNamespace.ToString(),
                Usings = syntaxNode.GetUsingStatements().Select((st) => st.ToString()).ToArray(),
                Methods = methods.ToArray()
            };

            return info;
        }

        return null;
    }


    private MethodInfo MapMethod(SyntaxNode node, SemanticModel semanticModel) {

        var mdsModel = semanticModel.GetDeclaredSymbol(node);
        if (mdsModel == null)
            return null;
        
        if (!mdsModel.GetAttributes().HasAttribute(typeof(InterfaceAttribute)))
            return null;
        
        var methodInfo = new MethodInfo();
        if (node is MethodDeclarationSyntax mds) {
            methodInfo.Name = mds.Identifier.ToString();
            methodInfo.Return = mds.ReturnType.ToString();
            methodInfo.ParameterList = mds.ParameterList.ToString();
        }
        return methodInfo;
    }
}