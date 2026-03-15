## Why

The source generator currently lacks the implementation to effectively intercept method calls using C# 14 Interceptors. This change is needed to complete the core functionality of the `InterceptorFramework`, allowing users to redirect method calls (like `Console.WriteLine`) to custom interceptor methods via a simple attribute-based approach.

## What Changes

- **Interceptor Attribute Resolution**: Complete the logic to identify methods decorated with the `[Interceptor]` attribute and extract target type and method information.
- **Call Site Identification**: Implement robust scanning of the compilation to find all matching call sites for the intercepted methods.
- **Source Generation of `[InterceptsLocation]`**: Generate the required `System.Runtime.CompilerServices.InterceptsLocationAttribute` and apply it to partial method declarations that wrap the interceptor logic.
- **Recursion Protection**: Ensure that method calls within the body of an interceptor are not themselves intercepted to prevent infinite recursion (StackOverflowException).
- **Project Configuration**: Update target frameworks and project settings to ensure compatibility with C# 14 and .NET 10.0.

## Capabilities

### New Capabilities
- `method-interception`: Implementation of the core source generator logic for intercepting method calls based on the `[Interceptor]` attribute.
- `interception-recursion-prevention`: Logic to exclude call sites within interceptor method bodies from the interception process.

### Modified Capabilities
- None: No existing capabilities are being modified as the framework is currently in an early, non-functional state.

## Impact

- `InterceptorSourceGenerator.cs`: Major implementation changes to the `Initialize` and `GenerateCode` methods.
- `InterceptorFramework.csproj`: Potential updates to Roslyn dependencies and project properties.
- `InterceptorFramework.Tests.csproj`: Update target framework to `net10.0`.
- `InterceptorFramework.Sample.csproj`: Update target framework to `net10.0`.
