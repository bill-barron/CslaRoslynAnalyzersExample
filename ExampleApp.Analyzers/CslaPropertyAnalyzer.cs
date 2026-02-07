using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace ExampleApp.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class CslaPropertyAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(CslaDiagnostics.NoCslaProperties);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(
                AnalyzeProperty,
                SymbolKind.Property);
        }

        private static void AnalyzeProperty(SymbolAnalysisContext context)
        {
            var property = (IPropertySymbol)context.Symbol;

            if (property.DeclaredAccessibility != Accessibility.Public && property.DeclaredAccessibility != Accessibility.Protected)
                return;

            if (IsCslaBased(property.Type))
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        CslaDiagnostics.NoCslaProperties,
                        property.Locations[0],
                        property.Name,
                        property.Type.ToDisplayString()));
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
