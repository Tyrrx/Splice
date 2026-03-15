## ADDED Requirements

### Requirement: Enhanced Package Metadata
The `Splice` NuGet package SHALL include the following metadata:
- Authors: Tyrrx
- Repository URL: https://github.com/Tyrrx/Splice
- Repository Type: git
- Package License Expression: MIT
- Project URL: https://github.com/Tyrrx/Splice
- Description: A C# .NET library for Roslyn Source Generators implementing C# 14 Interceptors.
- Tags: roslyn;sourcegen;interceptors;csharp14

#### Scenario: Package metadata verification
- **WHEN** the project is packed using `dotnet pack`
- **THEN** the resulting `.nupkg` contains the specified author, repository, and license information in its `.nuspec` file

### Requirement: Inclusion of README and LICENSE
The `Splice` NuGet package SHALL include the `README.md` and `LICENSE` files from the repository root.

#### Scenario: Documentation files in package
- **WHEN** the project is packed using `dotnet pack`
- **THEN** the `README.md` and `LICENSE` files are present in the package root and correctly referenced as the package's readme and license file in the `.nuspec`

### Requirement: Enable Package Generation
The `Splice` project SHALL be configured to generate a NuGet package on build in Release configuration or when explicitly requested.

#### Scenario: Automated package generation
- **WHEN** running `dotnet build -c Release`
- **THEN** a `.nupkg` file is automatically generated in the output directory
