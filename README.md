# Splice 🚀

![GitHub License](https://img.shields.io/github/license/Tyrrx/Splice)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/Tyrrx/Splice/CI)

A C# .NET solution focused on Roslyn Source Generators, specifically implementing C# 14 "Interceptors". This framework allows you to easily intercept and replace method calls at compile time.

## Quick Start 🏁

### 1. Installation 📦

Add the `Splice` as a source generator to your project. If you are using NuGet:

```sh
dotnet add package Splice
```

### 2. Project Configuration ⚙️

Interceptors are a C# 11+ feature (fully matured in C# 14). You need to enable them in your `.csproj` (C# 12+ required for basic support, C# 14 for full stability):

```xml
<PropertyGroup>
  <!-- Required: Interceptors are a preview/new feature (C# 14.0 or preview) -->
  <LangVersion>preview</LangVersion>
  
  <!-- Required: You must explicitly allow namespaces to contain interceptors -->
  <InterceptorsNamespaces>$(InterceptorsNamespaces);MyProject.Interceptors</InterceptorsNamespaces>
  
  <!-- Optional: Useful for debugging generated code -->
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
</PropertyGroup>
```

---

## How to Use 🛠️

### Create an Interceptor

To intercept a method, create a **partial class** and a **partial method** decorated with the `[Interceptor]` attribute.

```csharp
using System;
using Generators; // Namespace for the [Interceptor] attribute

namespace MyProject.Interceptors;

public static partial class MyInterceptor
{
    // Intercept Console.WriteLine(string) calls
    [Interceptor(typeof(Console), nameof(Console.WriteLine))]
    public static partial void WriteLine(string? message)
    {
        // NOTE: Calls to the intercepted method within its OWN body are NOT intercepted.
        // However, if this method calls ANOTHER method that then calls Console.WriteLine,
        // ⚠️ you might run into infinite recursion!
        Console.WriteLine($"[Intercepted] {message}");
    }
}
```

### Attribute Variations

The framework provides two ways to specify the target method:

<details>
<summary><b>1. Type-based (Recommended for non-generic targets)</b></summary>

```csharp
[Interceptor(typeof(TargetType), "MethodName")]
```
</details>

<details>
<summary><b>2. Generic-based (Clean syntax)</b></summary>

```csharp
[Interceptor<TargetType>("MethodName")]
```
</details>

---

## Configuration Details ⚙️

### The `InterceptorsNamespaces` Property

The C# compiler requires you to explicitly list the namespaces that are allowed to contain interceptors for security and performance reasons. 

If your interceptor is in `MyProject.Interceptors`, you **must** add that namespace to the `<InterceptorsNamespaces>` property in your `.csproj`. You can add multiple namespaces separated by semicolons.

```xml
<InterceptorsNamespaces>$(InterceptorsNamespaces);Namespace1;Namespace2</InterceptorsNamespaces>
```

---

## Features ✨
- **Core Interceptor Source Generator**: Automatically maps interceptors to their call sites.
- **Recursion Protection**: Prevents an interceptor from intercepting itself (only within its own method body).
- **Sample project**: Real-world demonstration of interception.
- **Comprehensive test suite**: Using xUnit v3 to ensure stability.

## Automated Workflows 🤖

### Continuous Integration (CI)
The project uses GitHub Actions to automatically build and test every push and pull request targeting the `main` branch.

### Continuous Deployment (CD)
Releases are automated via GitHub Actions. Pushing a tag matching `v*` (e.g., `v1.0.0`) triggers:
1. Automated build and test.
2. Creation of a GitHub Release with build artifacts.
3. Publishing the package to NuGet.org.

**Note**: To publish to NuGet, a `NUGET_API_KEY` must be configured in the repository's GitHub Secrets.

