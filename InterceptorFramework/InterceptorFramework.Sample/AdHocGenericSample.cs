namespace InterceptorFramework.Sample;

public partial class AdHocGenericSample
{
    public void AdHocGenericMethod<T>(T value)
    {
        Console.WriteLine($"Original {nameof(AdHocGenericMethod)} called with value: {value}");
    }
}