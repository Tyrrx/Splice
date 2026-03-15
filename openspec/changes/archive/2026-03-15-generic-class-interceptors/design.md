## Context

The `InterceptorSourceGenerator` needs to align with the C# interceptor specification regarding generic arity and containment. According to the specification, interceptors must either be non-generic or have an arity equal to the sum of the original method's arity and the arity of all its containing types (ordered from outermost to innermost).

## Goals / Non-Goals

**Goals:**
- Implement the "Arity" rule from the C# specification for interceptors targeting methods in generic classes.
- Ensure type parameter ordering: `T_OutermostClass, ..., T_InnermostClass, T_Method`.
- Support non-generic interceptors for generic targets if applicable (per specification).
- Correctly handle the `this` parameter for extension method interceptors targeting instance methods in generic classes.

**Non-Goals:**
- Supporting interceptors declared *within* generic classes (explicitly forbidden by specification).
- Supporting interceptors declared in non-static classes (interceptors must be static and top-level or in static classes to be useful/standard).

## Decisions

- **Arity Calculation**: The generator will traverse up the target method's parent symbols to sum the arity of all containing types.
- **Symbol Matching**: Use `OriginalDefinition` for both class and method when validating targets.
- **Type Parameter Ordinal Mapping**: Match target type parameters to interceptor type parameters based on their global ordinal (class params first, then method params).
- **Validation**: Add a diagnostic if an interceptor is declared within a generic type, as this will result in a compile-time error in the generated code anyway.

## Risks / Trade-offs

- [Risk] → **Complexity in Nesting**: Correctly ordering parameters for deeply nested types (e.g., `Outer<T1>.Inner<T2>.Method<T3>`).
- [Mitigation] → Recursive parent symbol traversal to gather all type parameters.
- [Risk] → **Signature Mismatch Errors**: Interceptors that don't match the required combined arity will fail.
- [Mitigation] → Provide clear feedback through diagnostics if the interceptor signature doesn't meet the arity requirement.
