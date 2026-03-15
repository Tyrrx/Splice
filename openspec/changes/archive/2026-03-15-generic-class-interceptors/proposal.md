## Why

Currently, the InterceptorFramework supports intercepting methods, but it does not fully support members of generic classes where the interceptor itself needs to match the combined generic signature of the class and the method. The C# interceptor specification requires that interceptors must be non-generic OR have arity equal to the sum of the original method's arity and all containing type arities. This change ensures the framework adheres to the specification while supporting these complex generic scenarios.

## What Changes

- Support for intercepting methods within generic classes by correctly calculating required interceptor arity.
- Updated `InterceptorAttribute` logic to handle the mapping of class-level type parameters to interceptor type parameters.
- Enhanced source generation to produce interceptors with the correct generic signature (sum of class and method arities) as required by the C# 11/12/13/14 specification.
- Validation that interceptors are not declared within generic types (as per specification).

## Capabilities

### New Capabilities
- `generic-class-interception`: Support for intercepting methods defined within generic classes, ensuring the interceptor's arity matches the sum of containing type arities and method arity, and type parameters are correctly ordered (outermost to innermost).

### Modified Capabilities
- `generic-method-interception`: Update existing generic method support to ensure compatibility with containing generic classes and verify arity requirements.

## Impact

- `InterceptorSourceGenerator`: Core logic for calculating arity and ordering type parameters in generated code.
- `InterceptorAttribute`: Documentation and validation updates for generic class context.
- `InterceptorFramework.Sample`: Add examples showing the correct arity for generic class members (e.g., `Interceptor<TClass, TMethod>`).
- `InterceptorFramework.Tests`: Comprehensive tests for nested generic contexts and arity validation.
