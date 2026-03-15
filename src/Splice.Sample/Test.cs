using System;

namespace Splice.Sample;

public class Test
{
    public static void TestCaller()
    {
        Console.WriteLine($"Direct call from {nameof(TestCaller)}");
        new Test().WriteLine("Hello, Interceptor!");
    }

    public void WriteLine(string message)
    {
        Console.WriteLine($"Implicit call in {nameof(TestCaller)} with {message}");
    }
}