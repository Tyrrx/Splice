## MODIFIED Requirements

### Requirement: Intercept Generic Methods
The `InterceptorSourceGenerator` MUST support intercepting calls to generic methods by generating a corresponding generic interceptor method. This includes generic methods defined in either generic or non-generic classes.

#### Scenario: Intercepting a simple generic method
- **WHEN** a call to `GenericMethod<T>(T value)` is decorated for interception
- **THEN** the generator MUST produce an interceptor method with the signature `[InterceptsLocation(...)] public static void InterceptGeneric<T>(this Target target, T value)`

### Requirement: Map Generic Type Arguments
The generated interceptor MUST correctly propagate generic type arguments from the original call site to ensure type safety and correct execution. This mapping must resolve both class-level and method-level generic parameters if they are present.

#### Scenario: Intercepting a generic method with multiple type parameters
- **WHEN** a call to `GenericMethod<TKey, TValue>(TKey key, TValue value)` is intercepted
- **THEN** the generated interceptor MUST include `<TKey, TValue>` in its signature and pass them to the intercepted call if applicable.
