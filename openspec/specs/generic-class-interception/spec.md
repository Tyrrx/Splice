## ADDED Requirements

### Requirement: Intercept Methods in Generic Classes
The `InterceptorSourceGenerator` MUST support intercepting methods defined within generic classes. The generated interceptor method MUST include the generic type parameters of the intercepted class in its own generic parameters.

#### Scenario: Intercepting a non-generic method in a generic class
- **WHEN** a call to `GenericClass<T>.Method(T value)` is decorated for interception
- **THEN** the generator MUST produce an interceptor method with the signature `public static void InterceptMethod<T>(this GenericClass<T> target, T value)`

### Requirement: Support Generic Methods in Generic Classes (Arity Summation)
The generator MUST support intercepting generic methods defined within generic classes, ensuring that the interceptor's arity is the sum of the arity of the containing types and the method itself.

#### Scenario: Intercepting a generic method in a generic class
- **WHEN** a call to `GenericClass<T1>.GenericMethod<T2>(T1 val1, T2 val2)` is intercepted
- **THEN** the generated interceptor MUST include both `<T1, T2>` in its signature, where T1 represents the class parameter and T2 represents the method parameter.

### Requirement: Order Type Parameters (Outermost to Innermost)
The generated interceptor MUST order its type parameters starting from the outermost containing type to the innermost containing type, followed by the method's own type parameters.

#### Scenario: Intercepting a method in nested generic classes
- **WHEN** a call to `Outer<T1>.Inner<T2>.Method<T3>(T1 v1, T2 v2, T3 v3)` is intercepted
- **THEN** the generated interceptor MUST have type parameters `<T1, T2, T3>`.

### Requirement: Attribute Placement for Generic Classes
The `InterceptsLocationAttribute` MUST correctly point to the call site of a method in a generic class, regardless of whether the class is instantiated with concrete types or other generic parameters.

#### Scenario: Intercepting call to GenericClass<int>.Method(int value)
- **WHEN** the generator identifies a call to `GenericClass<int>.Method(int value)`
- **THEN** it MUST emit the `InterceptsLocationAttribute` pointing to that specific call site.
