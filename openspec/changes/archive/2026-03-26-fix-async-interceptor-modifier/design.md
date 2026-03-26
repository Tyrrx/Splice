## Context

The `Splice.InterceptorSourceGenerator` generates `partial` method declarations for methods marked with the `[Interceptor]` attribute. These generated methods are required to have the `partial` modifier. If the original interceptor method is marked as `async`, the current generator implementation includes `async` in the generated signature.

## Goals / Non-Goals

**Goals:**
- Fix the compilation error by removing the `async` modifier from the generated `partial` method signature.
- Ensure the generated code is valid C# according to the rule: "The 'async' modifier can only be used in methods that have a body."

**Non-Goals:**
- Changing how the interceptor implementation itself is written or handled.
- Affecting other modifiers like `public`, `static`, or `partial`.

## Decisions

### 1. Filter Modifiers in `GetMethodSignature`
The most direct way to fix this is to filter out the `async` modifier when constructing the `modifiersString` in `GetMethodSignature`.

- **Decision:** Use `.Where(m => m != "async")` when processing the `modifiers` list.
- **Rationale:** This ensures the modifier is removed specifically from the generated signature without affecting the source interceptor's behavior.

## Risks / Trade-offs

- **[Risk]** Accidental removal of other necessary modifiers. → **Mitigation**: Use explicit string comparison for `"async"`.
- **[Risk]** Inconsistent signatures if other modifiers are added in the future. → **Mitigation**: The current approach of collecting modifiers from the source method is robust, only `async` needs special handling because it's only valid on bodies.
