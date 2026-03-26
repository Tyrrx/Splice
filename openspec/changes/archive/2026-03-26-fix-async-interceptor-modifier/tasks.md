## 1. Setup and Verification

- [x] 1.1 Create a test case in `Splice.Tests` that uses an `async` interceptor method to reproduce the issue (if it doesn't already exist or to confirm the fix).
- [x] 1.2 Verify that the test currently fails to compile or produces an invalid `.g.cs` file with the `async` modifier.

## 2. Generator Implementation

- [x] 2.1 Update `Splice.InterceptorSourceGenerator.GetMethodSignature` to filter out the `"async"` modifier from the `modifiers` list.

## 3. Verification

- [x] 3.1 Run the tests created in step 1.1 and ensure they now produce valid source code.
- [x] 3.2 Run all tests in the solution to ensure no regressions.
