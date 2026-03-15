## Why

The Splice NuGet package currently lacks essential metadata such as author, repository link, and license information. Additionally, the package output does not include the project's README or LICENSE file, which are standard for professional NuGet packages to provide context and legal clarity for consumers.

## What Changes

- Update `Splice.csproj` to include:
    - Author information.
    - Repository URL (https://github.com/Tyrrx/Splice).
    - License expression or file reference.
    - Description and tags for better discoverability.
    - Project URL.
- Configure `Splice.csproj` to include `README.md` and `LICENSE` files in the generated NuGet package (`.nupkg`).
- Enable package generation (`<GeneratePackageOnBuild>true</GeneratePackageOnBuild>` or ensure `IsPackable` is set correctly for release).

## Capabilities

### New Capabilities
- `nuget-metadata-extension`: Extends the Splice NuGet package with comprehensive metadata and documentation files (README, LICENSE).

### Modified Capabilities
<!-- Existing capabilities whose REQUIREMENTS are changing (not just implementation).
     Only list here if spec-level behavior changes. Each needs a delta spec file.
     Use existing spec names from openspec/specs/. Leave empty if no requirement changes. -->

## Impact

- `src/Splice/Splice.csproj`: Will be modified to add `<Package...>` properties and include items for the package.
- Build Process: Building the `Splice` project will now produce a richer NuGet package containing the README and LICENSE.
