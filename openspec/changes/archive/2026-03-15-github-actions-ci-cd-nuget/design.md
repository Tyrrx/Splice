## Context

The Splice project is a C# .NET solution. It currently has no automated CI/CD. We need to integrate GitHub Actions to automate building, testing, and releasing the source generator.

## Goals / Non-Goals

**Goals:**
- Automate build and test on push/PR to `main`.
- Automate GitHub Release and NuGet publishing on version tags (e.g., `v1.0.0`).
- Report test results directly in the GitHub Actions UI.
- Use standard .NET CLI tools for all operations.

**Non-Goals:**
- Automated deployment to an environment other than NuGet.
- Performance benchmarking in CI.
- Static analysis beyond standard build/test (e.g., no SonarQube for now).

## Decisions

### 1. Workflow Structure
- **Decision**: Split into two separate workflow files: `ci.yml` and `release.yml`.
- **Rationale**: Keeps the logic for daily integration separate from the more sensitive release logic. Easier to manage triggers and permissions.

### 2. CI Workflow (`ci.yml`)
- **Triggers**: `push` to `main` and `pull_request` to `main`.
- **Steps**:
    - Checkout code.
    - Setup .NET SDK (latest stable).
    - Restore dependencies.
    - Build solution (`Splice.slnx`).
    - Run tests with `dotnet test` and generate a TRX or JUnit report.
    - Use a GitHub Action (e.g., `dorny/test-reporter`) to surface test results.

### 3. Release Workflow (`release.yml`)
- **Triggers**: `push` of tags matching `v*`.
- **Steps**:
    - Checkout code.
    - Setup .NET SDK.
    - Build in Release configuration.
    - Pack the NuGet project.
    - Create a GitHub Release using `softprops/action-gh-release`.
    - Push the `.nupkg` to NuGet.org using `dotnet nuget push`.
- **Secrets**: Requires `NUGET_API_KEY` stored in GitHub Repository Secrets.

### 4. Versioning
- **Decision**: Use the tag name as the version for the NuGet package.
- **Rationale**: Simplifies the process by using the tag as the single source of truth for the release version.

## Risks / Trade-offs

- **[Risk]**: NuGet API key exposure. → **Mitigation**: Use GitHub Secrets and restrict the secret's availability to the release environment if possible.
- **[Risk]**: Build failure on tag. → **Mitigation**: Ensure CI passes on `main` before tagging. The release workflow will also run tests as a safeguard.
- **[Trade-off]**: Manual tagging requirement. → **Rationale**: Explicitly tagging a release is a standard and safe practice for versioning.
