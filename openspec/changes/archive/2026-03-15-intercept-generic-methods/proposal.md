## Why

Currently, the Splice successfully intercepts non-generic methods. However, it lacks support for ad-hoc generic methods (e.g., `Method<T>(T value)`). Supporting generic methods is essential for frameworks that rely on generic types for dependency injection, serialization, or logging, ensuring type safety and performance while allowing interception at specific call sites.

## What Changes

- **Source Generator Update**: Modify the `InterceptorSourceGenerator` to correctly identify and generate interceptors for generic methods.
- **Generic Parameter Mapping**: Implement logic to map generic type arguments from the original call site to the generated interceptor method.
- **Attribute Support**: Ensure `InterceptsLocationAttribute` works correctly with generic method signatures.
- **Sample Expansion**: Add examples of generic method interception to `Splice.Sample`.

## Capabilities

### New Capabilities
- `generic-method-interception`: Enables the source generator to intercept calls to generic methods by generating matching generic interceptors.

### Modified Capabilities
- (None)

## Impact

- **Splice**: Core generator logic will be updated to handle generic method symbols and syntax.
- **Splice.Sample**: Will include new test cases for generic interception.
- **Performance**: Minimal impact on generation time; runtime performance remains optimized as per C# interceptor standards.
