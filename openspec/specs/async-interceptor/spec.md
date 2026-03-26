## ADDED Requirements

### Requirement: Async modifier MUST NOT be generated
The `Splice.InterceptorSourceGenerator` SHALL exclude the `async` modifier from the generated `partial` method declaration in the `.g.cs` file.

#### Scenario: Interceptor method is async
- **WHEN** an interceptor method is defined as `[Interceptor] public static async Task InterceptAsync(...)`
- **THEN** the generated `partial` method signature in the source-generated file MUST BE `public static partial Task InterceptAsync(...)`
