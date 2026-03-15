## 1. Research & Analysis

- [x] 1.1 Verify current `SymbolEqualityComparer` usage in `InterceptorSourceGenerator`.
- [x] 1.2 Identify the exact points in `GenerateCode` where containing type and parameter matching fails for generics.
- [x] 1.3 Review the Roslyn `GetInterceptableLocation` API to ensure correctly extracted metadata for generic locations.

## 2. Testing Setup

- [x] 2.1 Create a new test file `GenericClassInterceptionTests.cs`.
- [x] 2.2 Add a failing test case for intercepting a non-generic method in a generic class.
- [x] 2.3 Add a failing test case for intercepting a generic method in a generic class (verifying sum arity requirement).
- [x] 2.4 Add a test case for an extension method interceptor targeting a generic class.
- [x] 2.5 Add a test case for nested generic classes (e.g., `Outer<T1>.Inner<T2>.Method<T3>`).

## 3. Core Implementation

- [x] 3.1 Update `GenerateCode` to use `OriginalDefinition` and `SymbolEqualityComparer` for containing type matching.
- [x] 3.2 Implement arity summation logic: calculate total required arity from all containing generic types and the method itself.
- [x] 3.3 Enhance parameter type matching logic to handle `TypeParameter` from containing classes, using ordinals from the combined set.
- [x] 3.4 Ensure `GetMethodSignature` correctly captures and emits the combined generic parameters and constraints.
- [x] 3.5 Add validation to detect and report if an interceptor is declared within a generic type (disallowed by spec).
- [x] 4.1 Run all tests and ensure they pass.
- [x] 4.2 Update `InterceptorFramework.Sample` with a generic class interception example.
- [x] 4.3 Verify that existing non-generic interceptions still work (regression testing).
