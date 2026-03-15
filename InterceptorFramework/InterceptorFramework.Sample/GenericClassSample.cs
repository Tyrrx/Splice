namespace InterceptorFramework.Sample;

public class GenericClassSample<T>
{
    public void NonAdHocGenericMethod(T value)
    {
        Console.WriteLine($"Original {nameof(NonAdHocGenericMethod)} called with value: {value}");
    }
    
    public void AdHocGenericMethod<TValue>(TValue value)
    {
        Console.WriteLine($"Original {nameof(AdHocGenericMethod)} called with value: {value} and generic class type: {typeof(T).Name}");
    }
}