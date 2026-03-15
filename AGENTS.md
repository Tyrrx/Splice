# Agent Guide: InterceptorFramework

This guide provides essential information for AI agents working on the InterceptorFramework repository.

## Project Overview
InterceptorFramework is a C# .NET solution focused on Roslyn Source Generators, specifically implementing C# 14 "Interceptors". It consists of the core generator, a sample project, and a test suite.

## Tech Stack
- **Languages:** C# (Latest / 14)
- **Frameworks:** .NET 10.0 (Tests & Sample), .NET Standard 2.0 (Generator)
- **Libraries:** Microsoft.CodeAnalysis (Roslyn), xUnit v3
- **Tools:** dotnet CLI

## Essential Commands

### Build & Restore
```bash
# Restore dependencies
dotnet restore

# Build the entire solution
dotnet build InterceptorFramework.slnx

# Build a specific project
dotnet build InterceptorFramework/InterceptorFramework/InterceptorFramework.csproj
```

### Testing
```bash
# Run all tests
dotnet test InterceptorFramework.Tests/InterceptorFramework.Tests.csproj

# Run a specific test class
dotnet test --filter "FullyQualifiedClassName"

# Run a specific test method
dotnet test --filter "Name=TestMethodName"

# Run tests with console output
dotnet test --logger "console;verbosity=detailed"
```

### Linting & Formatting
No explicit linter (like StyleCop) is configured in `.csproj`, but follow standard .NET conventions.
```bash
# Check formatting (if dotnet-format is installed)
dotnet format InterceptorFramework.slnx --verify-no-changes
```

## Code Style & Guidelines

### Naming Conventions
- **Classes/Methods/Properties:** PascalCase (e.g., `InterceptorSourceGenerator`, `GenerateCode`)
- **Private Fields:** camelCase (e.g., `myField`). DO NOT use underscore prefix.
- **Parameters/Local Variables:** camelCase (e.g., `semanticModel`, `context`)
- **Namespaces:** Match folder structure (e.g., `namespace InterceptorFramework;`)

### Formatting
- Use file-scoped namespaces (e.g., `namespace InterceptorFramework;`).
- Use `var` when the type is obvious from the right side of the assignment.
- Prefer expression-bodied members for simple methods and properties.
- Prefer LINQ over loops when a value is returned.
- Prefer functional programming styles over imperative programming styles when possible.
- Prefer expressions over statements when a value is returned.
- Braces on new lines (Allman style).

### Types & Nullability
- **Nullable Reference Types:** Enabled project-wide (`<Nullable>enable</Nullable>`). Use `?` for nullable types.
- **Modern C#:** Use C# 14 features like Primary Constructors where appropriate (e.g., `InterceptsLocationAttribute(int version, string contentHash, string filePath, int line, int column)`).
- **Collections:** Use `ImmutableArray<T>` or `IEnumerable<T>` for public APIs and internal logic where immutability is preferred, especially in Source Generators.

### Source Generator Specifics
- **Incremental Generators:** Always implement `IIncrementalGenerator`.
- **Performance:** Be extremely careful with `SyntaxProvider`. Only propagate equatable data through the pipeline to ensure efficient caching. Avoid holding onto `Symbol` or `SyntaxNode` objects in later stages of the pipeline if possible.
- **Source Text:** Use `UTF8` encoding and `SourceText.From` for adding generated sources.

### Error Handling
- Use standard exceptions (e.g., `ArgumentNullException`, `InvalidOperationException`).
- In Source Generators, use `Diagnostic` API for reporting errors/warnings to the user instead of throwing exceptions (though some existing code throws for "Bug" conditions).

### Imports
- Organize imports alphabetically, grouping `System` namespaces first.
- Remove unused using directives.

## Repository Structure
- `InterceptorFramework/InterceptorFramework/`: The core Roslyn Source Generator.
- `InterceptorFramework/InterceptorFramework.Sample/`: A project demonstrating usage of the interceptors.
- `InterceptorFramework.Tests/`: Unit tests using xUnit.

## External Rules
- **Cursor/Copilot:** No specific `.cursorrules` or `.github/copilot-instructions.md` found. Adhere to these AGENTS.md guidelines as the primary source of truth.
