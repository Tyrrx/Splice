using System;

namespace InterceptorFramework.Sample;

public class Test
{
    public static void TestCaller()
    {
        Console.WriteLine($"{nameof(TestCaller)}");
        new Test().WriteLine("aa");
    }

    public void WriteLine(string message)
    {
        Console.WriteLine($"[Test] {message}");
    }
}