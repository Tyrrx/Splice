# InterceptorFramework

![CI](https://github.com/drtz/InterceptorFramework/actions/workflows/ci.yml/badge.badge.svg)

A C# .NET solution focused on Roslyn Source Generators, specifically implementing C# 14 "Interceptors".

## Features
- Core Interceptor Source Generator
- Sample project demonstrating usage
- Comprehensive test suite using xUnit v3

## Automated Workflows

### Continuous Integration (CI)
The project uses GitHub Actions to automatically build and test every push and pull request targeting the `main` branch.

### Continuous Deployment (CD)
Releases are automated via GitHub Actions. Pushing a tag matching `v*` (e.g., `v1.0.0`) triggers:
1. Automated build and test.
2. Creation of a GitHub Release with build artifacts.
3. Publishing the package to NuGet.org.

**Note**: To publish to NuGet, a `NUGET_API_KEY` must be configured in the repository's GitHub Secrets.
