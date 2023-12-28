using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BenchSource
{
    internal static class SyntaxNodeUtils
    {
        public static NamespaceDeclarationSyntax GetNearestNamespace(this SyntaxNode node)
        {
            if (node is NamespaceDeclarationSyntax nds)
            {
                return nds;
            }
            return node.Parent?.GetNearestNamespace();
        }
    }
}
