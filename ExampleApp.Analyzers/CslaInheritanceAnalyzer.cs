using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ExampleApp.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class CslaInheritanceAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(CslaDiagnostics.NoCslaInheritance);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(
                AnalyzeNamedType,
                SymbolKind.NamedType);
        }

        private static void AnalyzeNamedType(SymbolAnalysisContext context)
        {
            var type = (INamedTypeSymbol)context.Symbol;

            // Only care about classes
            if (type.TypeKind != TypeKind.Class)
                return;

            var baseType = type.BaseType;

            while (baseType != null)
            {
                var ns = baseType.ContainingNamespace?.ToDisplayString();

                if (ns != null && ns.StartsWith("Csla"))
                {
                    var diagnostic = Diagnostic.Create(
                        CslaDiagnostics.NoCslaInheritance,
                        type.Locations[0],
                        type.Name);

                    context.ReportDiagnostic(diagnostic);
                    return;
                }

                baseType = baseType.BaseType;
            }
        }
    }
}
