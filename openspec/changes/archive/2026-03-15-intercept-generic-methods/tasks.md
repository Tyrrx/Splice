## 1. Preparation and Testing

- [x] 1.1 Add a new test case in `Splice.Tests` that attempts to intercept a generic method.
- [x] 1.2 Update `Splice.Sample` with a generic method and a corresponding interceptor to verify the failure/current state.

## 2. Source Generator Enhancements

- [x] 2.1 Update `GetMethodSignature` in `InterceptorSourceGenerator.cs` to correctly handle generic type parameters and potentially generic constraints if needed for the `partial` declaration.
- [x] 2.2 Refine the candidate filtering logic in `GenerateCode` to ensure arity and generic parameter types are correctly matched between the target and the interceptor.
- [x] 2.3 Verify `InterceptsLocationAttribute` generation for generic call sites, ensuring it points to the correct location.

## 3. Verification and Polishing

- [x] 3.1 Run all tests to ensure both non-generic and generic interception works as expected.
- [x] 3.2 Verify the generated code for generic interceptors in the `obj/` directory or via the Sample project.
- [x] 3.3 Ensure no regressions in existing interception scenarios.
