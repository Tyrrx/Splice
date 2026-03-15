using System;
using InterceptorFramework.Sample;

Console.WriteLine("Direct Call");
Test.TestCaller();
new AdHocGenericSample().AdHocGenericMethod(42);
new AdHocGenericSample().AdHocGenericMethod("Hello, Generics!");