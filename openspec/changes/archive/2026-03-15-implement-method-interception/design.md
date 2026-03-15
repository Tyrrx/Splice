## Context

The `InterceptorFramework` project provides a source generator for C# 14 Interceptors. Currently, the generator has a basic structure but lacks the critical logic to:
1.  Correctly identify call sites for intercepted methods across the compilation.
2.  Generate the necessary `[InterceptsLocation]` attributes with accurate file paths, line numbers, and column offsets.
3.  Prevent infinite recursion when an interceptor method calls the original method it is intercepting.

The generator uses the `IIncrementalGenerator` API from Roslyn.

## Goals / Non-Goals

**Goals:**
-   **Accurate Interception**: Every call to a target method (e.g., `Console.WriteLine`) should be redirectable to a user-defined interceptor.
-   **Source Generation of Metadata**: Automatically generate the `System.Runtime.CompilerServices.InterceptsLocationAttribute` if it's not already present in the compilation.
-   **Safe Interception**: Prevent StackOverflow by detecting and excluding call sites that exist within the bodies of interceptor methods.
-   **Modern Runtime Support**: Ensure the generator works with .NET 10.0 and C# 14 features.

**Non-Goals:**
-   Supporting interception of constructors or properties (interceptors only work for method calls in C# 14).
-   Performance optimization of the Roslyn compilation itself beyond standard incremental generator practices.

## Decisions

-   **Decision 1: Call Site Scanning Strategy**
    -   **Choice**: Use `SyntaxProvider.CreateSyntaxProvider` to identify all `InvocationExpressionSyntax` nodes.
    -   **Rationale**: This allows for an efficient incremental approach to finding potential interception points. We will filter nodes that match the target method name and signature defined in the `[Interceptor]` attribute.
    -   **Alternatives**: Scanning the entire semantic model in the final stage (too slow and breaks incrementality).

-   **Decision 2: Recursion Prevention**
    -   **Choice**: Compare the `SyntaxTree` and `TextSpan` of the call site against the `SyntaxTree` and `TextSpan` of the interceptor method body.
    -   **Rationale**: If a call site is found within the syntax tree of an interceptor method, it must be excluded from interception to allow the interceptor to call the original method (or other methods) without triggering itself.
    -   **Alternatives**: Using a special attribute to mark methods that shouldn't be intercepted (more complex for the user).

-   **Decision 3: InterceptsLocation Generation**
    -   **Choice**: Generate the newer hashed version of `InterceptsLocationAttribute` in the `PostInitializationOutput` phase.
    -   **Rationale**: C# 14/Roslyn uses a version of the attribute that includes a hash of the content to ensure source mapping reliability.
    -   **Alternatives**: Using the older line/column-only version (no longer supported/recommended for latest Interceptors).

## Risks / Trade-offs

-   **[Risk]**: Inaccurate location or hash mapping leading to compilation errors.
    -   **Mitigation**: Use `SemanticModel.GetInterceptableLocation` (available in newer Roslyn versions) to obtain the `contentHash`, `line`, and `column` required for the attribute.
-   **[Risk]**: Performance degradation in large solutions with many method calls.
    -   **Mitigation**: Aggressive filtering in the early stages of the incremental pipeline to ensure only relevant invocations are passed to the code generation phase.
-   **[Risk]**: Name collisions with generated partial classes.
    -   **Mitigation**: Use the existing class name and apply `partial` keywords consistently, ensuring the namespace matches.
