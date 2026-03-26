## Why

When an interceptor method is defined as `async`, the source generator currently includes the `async` modifier in the generated `partial` method declaration. However, C# does not allow the `async` modifier on method declarations that do not have a body (like partial method signatures). This results in a compilation error: "The 'async' modifier can only be used in methods that have a body."

## What Changes

- Modified the source generator to exclude the `async` modifier from the generated `partial` method signature.
- The `async` modifier will still remain on the implementation side of the partial method (the user-defined interceptor), which is correct and required for asynchronous execution.

## Capabilities

### New Capabilities
- None

### Modified Capabilities
- None

## Impact

- `Splice.InterceptorSourceGenerator`: Updated `GetMethodSignature` to filter out the `async` modifier.
- Generated code: The `partial` method declaration in `.g.cs` files will no longer contain the `async` keyword.
