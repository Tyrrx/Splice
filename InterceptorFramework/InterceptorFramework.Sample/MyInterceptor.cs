using System;
using Generators;

namespace InterceptorFramework.Sample;

public static partial class MyInterceptor
{
    [Interceptor(typeof(Console), nameof(Console.WriteLine))]
    public static void WriteLine(string? message)
    {
        Console.WriteLine($"[MyInterceptor] {message}");
        Meh(Console.WriteLine);
    }

    // TODO attribute to ignore interception
    public static void Meh(Action<string> func)
    {
        func($"[MyInterceptor]");
    }
}