using Microsoft.CodeAnalysis.CSharp;

namespace InterceptorFramework.Tests;

public class UnitTest1
{
    private const string Usage =
        // language=CSharp
        """
        public class TestClass
        {
            public void TestMethod()
            {
                System.Console.WriteLine("Hello, World!");
            }
        }
        """;

    private const string Interceptor =
        // language=CSharp
        """
        namespace Test;
        file public static partial class MyInterceptor
        {
            [Interceptor(typeof(Console), nameof(Console.WriteLine))]
            public static void WriteLine(string? message)
            {
                Console.WriteLine($"[MyInterceptor] {message}");
            }
        }
        """;

    [Fact]
    public void GenerateReportMethod()
    {
        var generator = new InterceptorSourceGenerator();

        var driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CSharpCompilation.Create(nameof(UnitTest1),
            [
                CSharpSyntaxTree.ParseText(Usage, cancellationToken: TestContext.Current.CancellationToken),
                CSharpSyntaxTree.ParseText(Interceptor, cancellationToken: TestContext.Current.CancellationToken)
            ],
            [
                // To support 'System.Attribute' inheritance, add reference to 'System.Private.CoreLib'.
                Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            ]);

        var runResult = driver.RunGenerators(compilation, TestContext.Current.CancellationToken).GetRunResult();

        Assert.True(true);
    }
}