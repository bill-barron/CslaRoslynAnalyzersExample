# CSLA Roslyn Analyzers Example

Demonstrate how Roslyn Analyzers can point out usage of CSLA in the UI layer

## Overview

This repository demonstrates how to create custom Roslyn Analyzers to enforce architectural boundaries in .NET applications. Specifically, it shows how to prevent CSLA (Component-based Scalable Logical Architecture) business objects from leaking into the UI/presentation layer.

## What are Roslyn Analyzers?

Roslyn Analyzers are custom code analysis tools that run as part of the .NET compilation process. They:

- **Analyze code in real-time** as you type in Visual Studio
- **Provide immediate feedback** with squiggly underlines and warning messages
- **Enforce coding standards** and architectural rules automatically
- **Run during build** to catch violations in CI/CD pipelines
- **Are project-scoped** - they only affect projects that reference them

Unlike traditional code review or runtime checks, Roslyn Analyzers catch architectural violations at compile-time, making them an excellent tool for enforcing clean architecture patterns.

## The Problem

CSLA provides powerful business object frameworks, but mixing business layer concerns with UI/presentation layer code violates separation of concerns. Common violations include:

- **Inheritance**: UI classes inheriting from CSLA base classes
- **Properties**: UI classes exposing CSLA business objects as properties
- **Method Parameters**: UI methods accepting CSLA objects as parameters

These violations make code harder to test, tightly couple layers, and create dependencies that should not exist.

## The Solution: Three Custom Analyzers

This project includes three analyzers that detect and warn about CSLA usage violations:

### 1. **CSLA001: Inheritance Analyzer** (`CslaInheritanceAnalyzer`)
Detects when a class inherits from any CSLA base class.

```csharp
// This triggers CSLA001
public class WebCustomer : DataCustomer  // DataCustomer inherits from Csla.BusinessBase
{
}
```

**Warning**: *Class 'WebCustomer' inherits from a CSLA base class*

### 2. **CSLA002: Property Analyzer** (`CslaPropertyAnalyzer`)
Detects when public/protected properties expose CSLA-based types.

```csharp
// This triggers CSLA002
public class WebOrderPresenter
{
    public DataCustomer Customer { get; set; }  // DataCustomer is CSLA-based
}
```

**Warning**: *Property 'Customer' exposes CSLA-based type 'DataCustomer'*

### 3. **CSLA003: Method Parameter Analyzer** (`CslaMethodParameterAnalyzer`)
Detects when public/protected methods have CSLA-based parameters.

```csharp
// This triggers CSLA003
public class WebOrderPresenter
{
    public void ProcessOrder(DataCustomer customer)  // CSLA parameter
    {
        // ...
    }
}
```

**Warning**: *Method 'ProcessOrder' has parameter 'customer' of CSLA-based type 'DataCustomer'*

## Project Structure

```
ExampleApp/
   ExampleApp.Analyzers/          # The Roslyn analyzer project (.NET Standard 2.0)
      CslaInheritanceAnalyzer.cs
      CslaPropertyAnalyzer.cs
      CslaMethodParameterAnalyzer.cs
      CslaDiagnostics.cs          # Diagnostic descriptors
   ExampleApp.Data/               # Data layer with CSLA objects (.NET Framework 4.8)
      DataCustomer.cs             # CSLA BusinessBase
      DataChildCustomer.cs        # Inherits from DataCustomer
      DataOrder.cs                # Uses CSLA types freely (no warnings)

   ExampleApp.Web/                # UI/Web layer (.NET Framework 4.8)
      WebCustomer.cs              # Inherits from CSLA (CSLA001)
      WebOrderPresenter.cs        # Bad example - violates CSLA002 & CSLA003
      WebOrderViewModel.cs        # Good example - uses DTOs
```

## Key Demonstration Points

### Analyzer Scope
The analyzers **only analyze projects that reference them**:

- **ExampleApp.Data**: Does NOT reference the analyzers
  - Can freely use CSLA inheritance, properties, and parameters
  - No warnings appear (this is the data layer where CSLA belongs)

- **ExampleApp.Web**: DOES reference the analyzers
  - Any CSLA usage triggers warnings
  - Forces developers to use DTOs/ViewModels instead of business objects

### Good vs Bad Patterns

** Bad (WebOrderPresenter.cs)** - Violates architectural boundaries:
```csharp
public class WebOrderPresenter
{
    public DataCustomer Customer { get; set; }  // CSLA002
    
    public void ProcessOrder(DataCustomer customer)  // CSLA003
    {
        Customer = customer;
    }
}
```

** Good (WebOrderViewModel.cs)** - Proper separation:
```csharp
public class WebOrderViewModel
{
    public string OrderNumber { get; set; }
    public string CustomerName { get; set; }
    
    public void UpdateFromDto(OrderDto dto)
    {
        OrderNumber = dto.OrderNumber;
        CustomerName = dto.CustomerName;
    }
}
```

## Building and Testing

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.8 SDK
- .NET Standard 2.0 SDK

### Build
```bash
dotnet build
```

### See the Analyzers in Action

1. Open the solution in Visual Studio
2. Navigate to `ExampleApp.Web\WebOrderPresenter.cs`
3. Observe the warning squiggles under:
   - The `Customer` property (CSLA002)
   - The `customer` parameter in `ProcessOrder` (CSLA003)
   - Both parameters in `UpdateOrder` (CSLA003 - two warnings)

4. Navigate to `ExampleApp.Data\DataOrder.cs`
5. Notice NO warnings despite similar code - the analyzers aren't referenced here

## How the Analyzers Work

Each analyzer follows the same pattern:

1. **Register for symbol analysis** during compilation
2. **Walk the type hierarchy** to detect CSLA namespace (`Csla.*`)
3. **Report diagnostics** when violations are found
4. **Only analyze public/protected members** (private members are ignored)

Example from `CslaPropertyAnalyzer`:
```csharp
private static bool IsCslaBased(ITypeSymbol type)
{
    var current = type as INamedTypeSymbol;
    while (current != null)
    {
        if (current.ContainingNamespace?.ToDisplayString().StartsWith("Csla") == true)
            return true;
        current = current.BaseType;
    }
    return false;
}
```

## Extending the Analyzers

You can create additional analyzers by:

1. Adding a new diagnostic descriptor in `CslaDiagnostics.cs`
2. Creating a new analyzer class inheriting from `DiagnosticAnalyzer`
3. Registering appropriate symbol/syntax actions
4. Following the same CSLA detection pattern

## References

- [Roslyn Analyzers Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/tutorials/how-to-write-csharp-analyzer-code-fix)
- [CSLA .NET Framework](https://cslanet.com/)
- [Writing Custom Roslyn Analyzers](https://github.com/dotnet/roslyn-analyzers)

## License

This project is provided as a demonstration/educational example.