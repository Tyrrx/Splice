## 1. Project Configuration Updates

- [x] 1.1 Set `IsPackable` to `true` in `src/Splice/Splice.csproj`.
- [x] 1.2 Add NuGet metadata properties (Authors, PackageId, RepositoryUrl, RepositoryType, PackageLicenseExpression, ProjectUrl, Description, PackageTags) to `src/Splice/Splice.csproj`.
- [x] 1.3 Configure `README.md` and `LICENSE` inclusion in the package via `<None>` or `<Content>` items.
- [x] 1.4 Set `<PackageReadmeFile>README.md</PackageReadmeFile>` in `src/Splice/Splice.csproj`.

## 2. Verification

- [x] 2.1 Run `dotnet pack -c Release` on the `src/Splice/Splice.csproj` project.
- [x] 2.2 Verify the generated `.nupkg` contains the correct metadata using `dotnet nuget verify` or by inspecting the `.nuspec` inside the zip.
- [x] 2.3 Ensure `README.md` and `LICENSE` are present at the root of the `.nupkg`.
