using System;
using Generators;

namespace InterceptorFramework.Sample;

public static partial class MyInterceptor
{
    [Interceptor(typeof(Console), nameof(Console.WriteLine))]
    public static partial void InterceptWriteLine(string? message)
    {
        Console.WriteLine($"[{nameof(InterceptWriteLine)}]: {message}");
    }
    
    [Interceptor(typeof(AdHocGenericSample), nameof(AdHocGenericSample.AdHocGenericMethod))]
    public static partial void InterceptAdHocGenericMethod<T>(this AdHocGenericSample target, T value)
    {
        Console.WriteLine($"[{nameof(InterceptAdHocGenericMethod)}]: {value}");
        target.AdHocGenericMethod(value);
    }

    [Interceptor(typeof(GenericClassSample<>), nameof(GenericClassSample<>.NonAdHocGenericMethod))]
    public static partial void InterceptNonAdHocGenericMethodInGenericClass<T>(this GenericClassSample<T> target, T value)
    {
        Console.WriteLine($"[InterceptNonAdHocGenericMethodInGenericClass]: {value}");
        target.NonAdHocGenericMethod(value);
    }

}