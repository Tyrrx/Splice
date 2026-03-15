using System;
using Splice.Sample;

Console.WriteLine("Direct Call");
Test.TestCaller();
new AdHocGenericSample().AdHocGenericMethod(42);
new GenericClassSample<string>().NonAdHocGenericMethod("Hello, Generics!");
new GenericClassSample<string>().AdHocGenericMethod(42);