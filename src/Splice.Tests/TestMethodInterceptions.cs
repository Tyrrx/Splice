using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Text.RegularExpressions;

namespace Splice.Tests;

public partial class TestMethodInterceptions
{
    private const string Usage =
        // language=CSharp
        """
        using System;
        public class TestClass
        {
            public void TestMethod()
            {
                Console.WriteLine("Hello, World!");
            }
        }
        """;

    private const string Interceptor =
        // language=CSharp
        """
        using System;
        using Generators;
        namespace Test;
        public static partial class MyInterceptor
        {
            [Interceptor(typeof(Console), nameof(Console.WriteLine))]
            public static void WriteLine(string? message)
            {
                Console.WriteLine($"[MyInterceptor] {message}");
            }
        }
        """;

    [Fact]
    public void GenerateInterceptionMethod()
    {
        var generator = new InterceptorSourceGenerator().AsSourceGenerator();

        var driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CSharpCompilation.Create(nameof(TestMethodInterceptions),
            [
                CSharpSyntaxTree.ParseText(Usage, cancellationToken: TestContext.Current.CancellationToken),
                CSharpSyntaxTree.ParseText(Interceptor, cancellationToken: TestContext.Current.CancellationToken)
            ],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyName(new System.Reflection.AssemblyName("System.Runtime")).Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CodeAnalysis.CSharp.CSharpCompilation).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CodeAnalysis.Compilation).Assembly.Location)
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var runResult = driver.RunGenerators(compilation, cancellationToken: TestContext.Current.CancellationToken).GetRunResult();

        Assert.Empty(runResult.Diagnostics);
        
        var generatedSource = string.Join("\n---\n", runResult.GeneratedTrees.Select(t => t.ToString()));
        Assert.Contains("[InterceptsLocation", generatedSource);
        Assert.Contains("data", generatedSource);
        Assert.Single(ValidationRegex().Matches(generatedSource));
    }

    [Fact]
    public void PreventRecursion()
    {
        var usageSource = """
            using System;
            public class TestClass
            {
                public void TestMethod()
                {
                    Console.WriteLine("Hello, World!");
                }
            }
            """;

        var interceptorSource = """
            using System;
            using Generators;
            namespace Test;
            public static partial class MyInterceptor
            {
                [Interceptor(typeof(Console), nameof(Console.WriteLine))]
                public static void WriteLine(string? message)
                {
                    Console.WriteLine($"[MyInterceptor] {message}");
                }
            }
            """;

        var generator = new InterceptorSourceGenerator().AsSourceGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CSharpCompilation.Create(nameof(TestMethodInterceptions) + "Recursion",
            [
                CSharpSyntaxTree.ParseText(usageSource, cancellationToken: TestContext.Current.CancellationToken),
                CSharpSyntaxTree.ParseText(interceptorSource, cancellationToken: TestContext.Current.CancellationToken)
            ],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyName(new System.Reflection.AssemblyName("System.Runtime")).Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CodeAnalysis.CSharp.CSharpCompilation).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CodeAnalysis.Compilation).Assembly.Location)
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var runResult = driver.RunGenerators(compilation, cancellationToken: TestContext.Current.CancellationToken).GetRunResult();

        Assert.Empty(runResult.Diagnostics);
        
        var generatedSource = runResult.GeneratedTrees.Last().ToString();
        
        // Count how many InterceptsLocation attributes were generated
        var attributeCount = generatedSource.Split(["[InterceptsLocation"], System.StringSplitOptions.None).Length - 1;
        Assert.Equal(1, attributeCount); // Only the call in TestClass should be intercepted
    }

    [Fact]
    public void GenerateInterceptionMethod_Generic()
    {
        var usageSource = """
            using System;
            public class GenericTestClass
            {
                public void Run()
                {
                    GenericMethod<string>("Hello");
                }

                public void GenericMethod<T>(T value)
                {
                    Console.WriteLine(value);
                }
            }
            """;

        var interceptorSource = """
            using System;
            using Generators;
            namespace Test;
            public static partial class GenericInterceptor
            {
                [Interceptor(typeof(GenericTestClass), nameof(GenericTestClass.GenericMethod))]
                public static void InterceptGeneric<T>(this GenericTestClass target, T value)
                {
                    Console.WriteLine($"[Intercepted] {value}");
                }
            }
            """;

        var generator = new InterceptorSourceGenerator().AsSourceGenerator();
        var compilation = CSharpCompilation.Create(nameof(TestMethodInterceptions) + "Generic",
            [
                CSharpSyntaxTree.ParseText(usageSource, new CSharpParseOptions(LanguageVersion.CSharp12), cancellationToken: TestContext.Current.CancellationToken),
                CSharpSyntaxTree.ParseText(interceptorSource, new CSharpParseOptions(LanguageVersion.CSharp12), cancellationToken: TestContext.Current.CancellationToken)
            ],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyName(new System.Reflection.AssemblyName("System.Runtime")).Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CodeAnalysis.CSharp.CSharpCompilation).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CodeAnalysis.Compilation).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(AttributeUsageAttribute).Assembly.Location)
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true)
                .WithSpecificDiagnosticOptions(new Dictionary<string, ReportDiagnostic> { { "CS0618", ReportDiagnostic.Suppress } })
                .WithMetadataImportOptions(MetadataImportOptions.All));

        var driver = CSharpGeneratorDriver.Create(
            generators: [generator],
            parseOptions: new CSharpParseOptions(LanguageVersion.CSharp12));

        var runResult = driver.RunGenerators(compilation, cancellationToken: TestContext.Current.CancellationToken).GetRunResult();

        var generatedSource = string.Join("\n---\n", runResult.GeneratedTrees.Select(t => t.ToString()));
        if (!generatedSource.Contains("InterceptGeneric"))
        {
             Assert.Fail($"Generated sources did not contain interceptor. Trees: {runResult.GeneratedTrees.Length}. Content:\n{generatedSource}");
        }

        Assert.Empty(runResult.Diagnostics);
    }


    [GeneratedRegex("""\[InterceptsLocation\(version: 1, data: "[^"]+"\)\]""")]
    private static partial Regex ValidationRegex();
}
