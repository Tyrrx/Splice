using System;
using InterceptorFramework.Sample;

Console.WriteLine("Direct Call");
Test.TestCaller();
new GenericTest().GenericMethodToIntercept(42);
new GenericTest().GenericMethodToIntercept("Hello, Generics!");