using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace Splice.Tests;

public class GenericClassInterceptionTests
{
    private const string UsageSource = """
        using System;

        namespace TestNamespace
        {
            public class GenericClass<T>
            {
                public void NonGenericMethod(T value)
                {
                    Console.WriteLine($"Original: {value}");
                }

                public void GenericMethod<U>(T val1, U val2)
                {
                    Console.WriteLine($"Original: {val1}, {val2}");
                }
            }

            public class Consumer
            {
                public void Run()
                {
                    var instance = new GenericClass<string>();
                    instance.NonGenericMethod("Hello"); // L1
                    instance.GenericMethod<int>("World", 42); // L2
                }
            }
        }
        """;

    [Fact]
    public void InterceptNonGenericMethodInGenericClass()
    {
        var interceptorSource = """
            using System;
            using Splice;
            using TestNamespace;

            namespace TestNamespace
            {
                public static partial class Interceptor
                {
                    [Interceptor(typeof(GenericClass<string>), "NonGenericMethod")]
                    public static void InterceptNonGeneric<T>(this GenericClass<T> target, T value)
                    {
                        Console.WriteLine($"[Intercepted] {value}");
                    }
                }
            }
            """;

        var result = RunGenerator(interceptorSource);

        Assert.Empty(result.Diagnostics);
        var generatedSource = string.Join("\n---\n", result.GeneratedTrees.Select(t => t.ToString()));
        Assert.Contains("InterceptNonGeneric<T>", generatedSource);
        Assert.Contains("[InterceptsLocation", generatedSource);
    }

    [Fact]
    public void InterceptGenericMethodInGenericClass()
    {
        var interceptorSource = """
            using System;
            using Splice;
            using TestNamespace;

            namespace TestNamespace
            {
                public static partial class Interceptor
                {
                    [Interceptor(typeof(GenericClass<>), nameof(GenericClass<object>.GenericMethod))]
                    public static void InterceptGeneric<T, U>(this GenericClass<T> target, T val1, U val2)
                    {
                        Console.WriteLine($"[Intercepted] {val1}, {val2}");
                    }
                }
            }
            """;

        var result = RunGenerator(interceptorSource);

        Assert.Empty(result.Diagnostics);
        var generatedSource = string.Join("\n---\n", result.GeneratedTrees.Select(t => t.ToString()));
        Assert.Contains("InterceptGeneric<T, U>", generatedSource);
        Assert.Contains("[InterceptsLocation", generatedSource);
    }

    [Fact]
    public void InterceptNestedGenericClassMethod()
    {
        var usage = """
            namespace TestNamespace
            {
                public class Outer<T1>
                {
                    public class Inner<T2>
                    {
                        public void Method<T3>(T1 v1, T2 v2, T3 v3) { }
                    }
                }

                public class Consumer
                {
                    public void Run()
                    {
                        var inner = new Outer<int>.Inner<string>();
                        inner.Method<bool>(1, "2", true);
                    }
                }
            }
            """;

        var interceptorSource = """
            using System;
            using Splice;
            using TestNamespace;

            namespace TestNamespace
            {
                public static partial class Interceptor
                {
                    [Interceptor(typeof(Outer<>.Inner<>), "Method")]
                    public static void InterceptNested<T1, T2, T3>(this Outer<T1>.Inner<T2> target, T1 v1, T2 v2, T3 v3)
                    {
                    }
                }
            }
            """;

        var result = RunGeneratorWithUsage(usage, interceptorSource);

        Assert.Empty(result.Diagnostics);
        var generatedSource = string.Join("\n---\n", result.GeneratedTrees.Select(t => t.ToString()));
        Assert.Contains("InterceptNested<T1, T2, T3>", generatedSource);
    }

    private GeneratorDriverRunResult RunGeneratorWithUsage(string usageSource, string interceptorSource)
    {
        var generator = new InterceptorSourceGenerator().AsSourceGenerator();
        var compilation = CSharpCompilation.Create("TestCompilation",
            [
                CSharpSyntaxTree.ParseText(usageSource, new CSharpParseOptions(LanguageVersion.CSharp12)),
                CSharpSyntaxTree.ParseText(interceptorSource, new CSharpParseOptions(LanguageVersion.CSharp12))
            ],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyName(new System.Reflection.AssemblyName("System.Runtime")).Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CodeAnalysis.CSharp.CSharpCompilation).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CodeAnalysis.Compilation).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(AttributeUsageAttribute).Assembly.Location)
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(
            generators: [generator],
            parseOptions: new CSharpParseOptions(LanguageVersion.CSharp12));

        return driver.RunGenerators(compilation).GetRunResult();
    }

    private GeneratorDriverRunResult RunGenerator(string interceptorSource)
    {
        return RunGeneratorWithUsage(UsageSource, interceptorSource);
    }
}
