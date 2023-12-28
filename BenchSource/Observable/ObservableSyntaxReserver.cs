using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchSource.Observable
{
    public class ObservableSyntaxReserver : ISyntaxReceiver
    {
        public ClassDeclarationSyntax ClassToAugment { get; private set; }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Business logic to decide what we're interested in goes here
            if (syntaxNode is ClassDeclarationSyntax cds && cds.AttributeLists.Any(list => list.Attributes.Any(atb => atb.Name.GetText().ToString() == "Observable")))
            {
                ClassToAugment = cds;
            }
        }
    }
}
