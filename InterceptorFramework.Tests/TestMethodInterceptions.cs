using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace InterceptorFramework.Tests;

public class TestMethodInterceptions
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
        
        var generatedSource = runResult.GeneratedTrees.Last().ToString();
        Assert.Contains("[InterceptsLocation", generatedSource);
        Assert.Contains("contentHash", generatedSource);
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
}
