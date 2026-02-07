using Microsoft.CodeAnalysis;

namespace ExampleApp.Analyzers
{
    internal static class CslaDiagnostics
    {
        public static readonly DiagnosticDescriptor NoCslaInheritance =
            new DiagnosticDescriptor(
                id: "CSLA001",
                title: "CSLA inheritance is not allowed in this project",
                messageFormat: "Class '{0}' inherits from a CSLA base class",
                category: "Architecture",
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true);

        // RS2000 fix: Ensure all rules are registered in the analyzer's SupportedDiagnostics property.
        // No code change needed here, but you must update your analyzer class to include CSLA002 in SupportedDiagnostics.

        public static readonly DiagnosticDescriptor NoCslaProperties =
            new DiagnosticDescriptor(
                id: "CSLA002",
                title: "CSLA properties are not allowed in UI/MVP",
                messageFormat: "Property '{0}' exposes CSLA-based type '{1}'",
                category: "Architecture",
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor NoCslaMethodParameters =
            new DiagnosticDescriptor(
                id: "CSLA003",
                title: "CSLA method parameters are not allowed in public/protected methods",
                messageFormat: "Method '{0}' has parameter '{1}' of CSLA-based type '{2}'",
                category: "Architecture",
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true);
    }
}
