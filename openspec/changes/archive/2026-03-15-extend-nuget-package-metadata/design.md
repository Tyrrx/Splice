## Context

The `Splice` project is a C# Source Generator. It is currently configured with minimal NuGet metadata in `Splice.csproj`. To make it ready for public distribution on NuGet.org, it needs standard metadata and the inclusion of documentation files (README and LICENSE).

## Goals / Non-Goals

**Goals:**
- Add all standard NuGet metadata (Author, Repository, License, Description, Tags) to `Splice.csproj`.
- Include `README.md` and `LICENSE` in the generated NuGet package.
- Ensure the package is packable and generates correctly.

**Non-Goals:**
- Changing the library's functionality.
- Modifying the CI/CD pipeline (this design focuses on the project file configuration).
- Versioning logic (will use existing versioning or default).

## Decisions

### 1. License Specification
**Decision:** Use `<PackageLicenseExpression>MIT</PackageLicenseExpression>` in the `.csproj`.
**Rationale:** The project has an MIT LICENSE file at the root. Using a license expression is the modern and recommended way for NuGet packages when using standard licenses.
**Alternatives:** Including the license file itself (`<PackageLicenseFile>`), but expressions are preferred by NuGet.org for standard licenses. However, we will still include the `LICENSE` file in the package for completeness.

### 2. Including README and LICENSE
**Decision:** Use `<None>` or `<Content>` items with `Pack="true"` and `PackagePath=""`.
**Rationale:** This ensures the files are included in the root of the `.nupkg`. We will specifically use `<PackageReadmeFile>README.md</PackageReadmeFile>` for the README to be displayed on NuGet.org.

### 3. Repository Metadata
**Decision:** Include `<RepositoryUrl>`, `<RepositoryType>`, and `<PublishRepositoryUrl>`.
**Rationale:** Provides links to the source code and enables Source Link support if configured later.

## Risks / Trade-offs

- **Risk:** README paths might be tricky if not referenced correctly.
- **Mitigation:** Use relative paths from the project file to the root `../../README.md` and ensure `Pack="true"`.
