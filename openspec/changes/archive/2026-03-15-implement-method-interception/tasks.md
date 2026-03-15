## 1. Project Setup and Infrastructure

- [x] 1.1 Update Splice.Tests to target net10.0
- [x] 1.2 Update Splice.Sample to target net10.0
- [x] 1.3 Ensure C# 14 / latest language version is set in all project files
- [x] 1.4 Update AGENTS.md to clarify C# 14 and net10.0 requirements

## 2. Core Source Generator Implementation

- [x] 2.1 Update PostInitializationOutput to include InterceptorAttribute and InterceptsLocationAttribute definitions
- [x] 2.2 Implement robust identification of [Interceptor] attributes in MethodDeclarationSyntax
- [x] 2.3 Refine the SyntaxProvider pipeline to correctly find all InvocationExpressionSyntax call sites
- [x] 2.4 Implement recursion prevention: filter out call sites located within the body of an interceptor method
- [x] 2.5 Generate accurate hashed [InterceptsLocation] attributes with correct version, hash, file path, line, and column info via GetInterceptableLocation
- [x] 2.6 Emit partial class declarations that wrap the interceptor with the generated [InterceptsLocation] attributes

## 3. Verification and Testing

- [x] 3.1 Update UnitTest1.cs to verify that [InterceptsLocation] is correctly generated for a simple call
- [x] 3.2 Add a test case to verify that calls inside the interceptor method are NOT intercepted (recursion prevention)
- [x] 3.3 Verify the Sample project builds and successfully redirects Console.WriteLine to the interceptor
- [x] 3.4 Run all tests and ensure they pass on the net10.0 runtime
