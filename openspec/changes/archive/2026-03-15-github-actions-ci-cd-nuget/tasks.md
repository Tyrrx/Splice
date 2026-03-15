## 1. CI Workflow Setup

- [x] 1.1 Create `.github/workflows/ci.yml` file.
- [x] 1.2 Define `on` triggers for `push` and `pull_request` to `main`.
- [x] 1.3 Add steps for Checkout and .NET SDK setup.
- [x] 1.4 Add steps for `dotnet restore` and `dotnet build` of `InterceptorFramework.slnx`.
- [x] 1.5 Add step for `dotnet test` with TRX/JUnit logging.
- [x] 1.6 Integrate `dorny/test-reporter` or equivalent to surface test results.

## 2. CD Workflow Setup

- [x] 2.1 Create `.github/workflows/release.yml` file.
- [x] 2.2 Define `on` triggers for `push` of tags matching `v*`.
- [x] 2.3 Add steps for Checkout and .NET SDK setup.
- [x] 2.4 Add steps for `dotnet build` and `dotnet pack` using the tag version.
- [x] 2.5 Add step for GitHub Release creation using `softprops/action-gh-release`.
- [x] 2.6 Add step for `dotnet nuget push` to NuGet.org using `secrets.NUGET_API_KEY`.

## 3. Verification and Documentation

- [x] 3.1 Verify workflow syntax using `action-validator` or local dry-run if possible.
- [x] 3.2 Update `README.md` to include build/test status badges.
- [x] 3.3 Document the release process (how to tag and what happens next).
