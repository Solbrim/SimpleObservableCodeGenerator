using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using BenchSource.Models;
using Microsoft.CodeAnalysis.CSharp;

namespace BenchSource.Observable
{
    [Generator]
    public class ObservableGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            ObservableSyntaxReserver receiver = (ObservableSyntaxReserver)context.SyntaxReceiver;
            ClassDeclarationSyntax obvC = receiver.ClassToAugment;

            if (obvC == null)
            {
                return;
            }

            var _namespace = obvC.GetNearestNamespace()?.Name.ToString() ?? context.Compilation.GetEntryPoint(context.CancellationToken).ContainingNamespace.ToDisplayString();
            var fields = obvC.SyntaxTree.GetRoot().DescendantNodes().OfType<FieldDeclarationSyntax>()
                .SelectMany(f =>
                {
                    var type = f.Declaration.Type.GetText().ToString();
                    var identifiers = f.Declaration.Variables.Where(v => v.Identifier.ValueText.StartsWith("_")).Select(v => v.Identifier.ValueText);
                    return identifiers.Select(i => new FieldRecord(i.Trim(), type.Trim()));
                }).ToArray();
            var field_text = string.Join("\n", fields.Select(PubFunc));

            // add the generated implementation to the compilation
            SourceText sourceText = SourceText.From($@"
namespace {_namespace}
{{
    public partial class {obvC.Identifier}
    {{
        {field_text}
    }}
}}", Encoding.UTF8);
            context.AddSource($"{obvC.Identifier}.g.cs", sourceText);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ObservableSyntaxReserver());
        }

        string PubFunc (FieldRecord field)
        {
            var sb = new StringBuilder();
            sb.Append(PubProperty(field));
            sb.Append("\n");
            sb.Append(PubChanged(field));
            return sb.ToString();
        }

        string PubChanged (FieldRecord field)
        {
            return $"\tpublic Action<{field.Type}> {field.ChangedName} {{ get; set; }}";
        }

        string PubProperty (FieldRecord field)
        {
            return $@"public {field.Type} {field.CamelName} {{
            get => {field.Name};
            set {{
                {field.Name} = value;
                {field.ChangedName}.Invoke(value);
            }}
        }}";
        }
    }
}
