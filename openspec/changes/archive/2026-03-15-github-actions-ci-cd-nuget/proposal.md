## Why

Currently, the InterceptorFramework project lacks an automated CI/CD pipeline. Developers must manually build and test the source generator, which is error-prone and doesn't guarantee a fresh environment for verification. Additionally, publishing new releases and NuGet packages is a manual process that could be streamlined to ensure consistency and speed.

## What Changes

- Introduce a GitHub Actions workflow for Continuous Integration (CI) that builds and tests the solution on every push and pull request.
- Implement a release workflow that automates the creation of GitHub releases and publishing of NuGet packages when a new tag is created.
- Configure test reporting in GitHub Actions to provide immediate feedback on test results.

## Capabilities

### New Capabilities
- `ci-workflow`: Automated build, test, and reporting pipeline for the InterceptorFramework solution.
- `cd-workflow`: Automated release and NuGet publishing pipeline triggered by version tags.

### Modified Capabilities
- None

## Impact

- Adds `.github/workflows/` directory with YAML configurations.
- Requires NuGet API key to be stored in GitHub Secrets for package publishing.
- Affects the development workflow by providing automated feedback on PRs and streamlining the release process.
