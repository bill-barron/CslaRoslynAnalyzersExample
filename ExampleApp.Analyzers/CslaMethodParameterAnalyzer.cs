using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace ExampleApp.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class CslaMethodParameterAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(CslaDiagnostics.NoCslaMethodParameters);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(
                AnalyzeMethod,
                SymbolKind.Method);
        }

        private static void AnalyzeMethod(SymbolAnalysisContext context)
        {
            var method = (IMethodSymbol)context.Symbol;

            if (method.DeclaredAccessibility != Accessibility.Public && method.DeclaredAccessibility != Accessibility.Protected)
                return;

            foreach (var parameter in method.Parameters)
            {
                if (IsCslaBased(parameter.Type))
                {
                    context.ReportDiagnostic(
                        Diagnostic.Create(
                            CslaDiagnostics.NoCslaMethodParameters,
                            parameter.Locations[0],
                            method.Name,
                            parameter.Name,
                            parameter.Type.ToDisplayString()));
                }
            }
        }

        private static bool IsCslaBased(ITypeSymbol type)
        {
            var named = type as INamedTypeSymbol;
            if (named == null)
                return false;

            var current = named;
            while (current != null)
            {
                if (current.ContainingNamespace?.ToDisplayString().StartsWith("Csla") == true)
                    return true;

                current = current.BaseType;
            }

            return false;
        }
    }
}
