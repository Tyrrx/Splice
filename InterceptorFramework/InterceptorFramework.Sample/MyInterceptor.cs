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
    
    [Interceptor(typeof(GenericTest), nameof(GenericTest.GenericMethodToIntercept))]
    public static void GenericMethodToIntercept<T>(T value)
    {
        Console.WriteLine($"GenericMethodToIntercept called with value: {value}");
    }
    
}