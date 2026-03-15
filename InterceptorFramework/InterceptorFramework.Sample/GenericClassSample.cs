namespace InterceptorFramework.Sample;

public class GenericClassSample<T>
{
    public void NonAdHocGenericMethod(T value)
    {
        Console.WriteLine($"Original {nameof(NonAdHocGenericMethod)} called with value: {value}");
    }
}