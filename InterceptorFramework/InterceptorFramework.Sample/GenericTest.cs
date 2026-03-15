namespace InterceptorFramework.Sample;

public class GenericTest
{
    public void GenericMethodToIntercept<T>(T value)
    {
        throw new NotImplementedException("This method is intended to be intercepted and should not be called directly.");
    }
}