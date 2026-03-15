## Requirements

### Requirement: Release Workflow Triggered by Version Tags
The CD workflow SHALL execute when a git tag matching the pattern `v*` is pushed to the repository.

#### Scenario: Workflow triggered by tag
- **WHEN** a tag `v1.2.3` is pushed to GitHub
- **THEN** the GitHub Actions workflow `release.yml` starts executing

### Requirement: Automated NuGet Packaging
The CD workflow SHALL build and pack the `InterceptorFramework` project into a `.nupkg` file using the version specified in the git tag.

#### Scenario: Package creation
- **WHEN** the workflow executes `dotnet pack`
- **THEN** a `.nupkg` file is created with the tag name as its version

### Requirement: GitHub Release Creation
The CD workflow SHALL create a new GitHub Release corresponding to the version tag.

#### Scenario: Release created
- **WHEN** the workflow uses the GitHub API to create a release
- **THEN** a new release entry appears on GitHub for the pushed tag

### Requirement: NuGet.org Publishing
The CD workflow SHALL push the generated NuGet package to NuGet.org using the provided API key.

#### Scenario: Package published to NuGet
- **WHEN** the workflow executes `dotnet nuget push` using the `NUGET_API_KEY` secret
- **THEN** the package becomes available on NuGet.org
