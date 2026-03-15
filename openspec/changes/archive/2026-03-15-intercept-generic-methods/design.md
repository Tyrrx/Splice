## Context

The `InterceptorSourceGenerator` currently supports intercepting non-generic methods. It uses a `SyntaxProvider` to find invocations and a `GetMethodForSourceGen` method to identify methods marked with the `[Interceptor]` attribute. However, the current logic for matching interceptors with targets (in `GenerateCode`) and the signature generation (`GetMethodSignature`) needs to be enhanced to handle generic type parameters.

## Goals / Non-Goals

**Goals:**
- Enable interception of generic methods (e.g., `Void Generic<T>(T value)`).
- Ensure generated interceptors correctly include type parameters from the interceptor declaration.
- Match targets based on name, containing type, parameter types, AND arity (type parameter count).

**Non-Goals:**
- Supporting interceptors for methods where the interceptor itself is NOT generic but the target IS generic (or vice versa). We expect signatures to match.
- Handling complex generic constraints in the generated `partial` signature (C# interceptors have specific rules about where constraints can be declared; usually, they are in the original declaration, and the `partial` method just needs the type parameter names).

## Decisions

- **Arity-Based Matching**: In `GenerateCode`, we will ensure that `candidateMethodSymbol.Arity` matches `interceptionMethodSymbol.Arity`. (Already partially present but needs verification for generic context).
- **Type Parameter Propagation**: The `GetMethodSignature` method already includes `method.TypeParameterList?.ToFullString()`, which should correctly emit `<T>` or `<T1, T2>`. We must ensure this is sufficient for the compiler to link the `partial` method.
- **Symbol Matching**: Use `SymbolEqualityComparer.Default` or `IncludeNullability` for matching parameter types. When parameters use generic type parameters (e.g., `T`), they should match if both methods are generic with the same arity and the parameters use the same-indexed type parameters.

## Risks / Trade-offs

- **[Risk] Generic Constraints** → C# `partial` methods sometimes have restrictions on repeating constraints. 
  - *Mitigation*: The generated code will initially only include the type parameter list (e.g., `<T>`) without constraints. If constraints are required for the `partial` signature to be valid, we will need to extract them from `method.ConstraintClauses`.
- **[Risk] Overload Resolution** → Generic overloads (e.g., `Foo(T x)` and `Foo<T>(T x)`) might be confused.
  - *Mitigation*: Strict arity and parameter type checking in the `GenerateCode` candidate filter.
