using System;
using Generators;

namespace InterceptorFramework.Sample;

public static partial class MyInterceptor
{
    [Interceptor(typeof(Console), nameof(Console.WriteLine))]
    public static partial void WriteLine(string? message)
    {
        Console.WriteLine($"[MyInterceptor] {message}");
    }
    
}