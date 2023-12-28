using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace BenchSg.Observable
{
    [Generator]
    public class ObservableGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            ObservableSyntaxReserver receiver = (ObservableSyntaxReserver)context.SyntaxReceiver;
            ClassDeclarationSyntax observableClass = receiver.ClassToAugment;

            if (observableClass == null)
            {
                return;
            }

            // add the generated implementation to the compilation
            SourceText sourceText = SourceText.From($@"
public partial class {observableClass.Identifier}
{{
    private void GeneratedMethod()
    {{
        // generated code
    }}
}}", Encoding.UTF8);
            context.AddSource($"{observableClass.Identifier}.g.cs", sourceText);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ObservableSyntaxReserver());
        }
    }
}
