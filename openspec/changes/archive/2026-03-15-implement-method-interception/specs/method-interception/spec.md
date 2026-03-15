## ADDED Requirements

### Requirement: Identify Interceptable Call Sites
The system SHALL scan all source files in the compilation to identify call sites (invocations) of methods that match the targets specified in `[Interceptor]` attributes.

#### Scenario: Intercepting Console.WriteLine
- **WHEN** the generator finds a method decorated with `[Interceptor(typeof(Console), nameof(Console.WriteLine))]`
- **THEN** it SHALL locate all `Console.WriteLine` invocations in the project (excluding those inside the interceptor itself)

### Requirement: Generate [InterceptsLocation] Attributes
For each valid call site identified, the generator SHALL produce the hashed version of the `[InterceptsLocation]` attribute that includes the version, content hash, file path, line number, and column offset.

#### Scenario: Generating Attribute Code with Hash
- **WHEN** an invocation of `Console.WriteLine("hello")` is found at `Test.cs`, line 10, column 5
- **THEN** the generator SHALL emit `[InterceptsLocation(version: 1, contentHash: "<hash>", filePath: "Test.cs", line: 10, column: 5)]` on the corresponding partial method

### Requirement: Provide Interceptor Attribute Definition
The source generator SHALL automatically include the definition for the `[Interceptor]` attribute and the `[InterceptsLocation]` attribute in the generated output to ensure compilation success.

#### Scenario: Post-Initialization Generation
- **WHEN** the compilation starts
- **THEN** the generator SHALL emit the definition for `Generators.InterceptorAttribute` and `System.Runtime.CompilerServices.InterceptsLocationAttribute`
