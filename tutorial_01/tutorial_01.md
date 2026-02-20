## 🔽 Tutorial 01 🔒

**Test Driven Development**

---

### Task 1

Get informed about [Unit Testing](https://en.wikipedia.org/wiki/Unit_testing) and [Test Driven Development](https://en.wikipedia.org/wiki/Test-driven_development) (TDD). One additional tutorial can be found at [blog.cellenza.com](http://blog.cellenza.com/publications/articles/tutorial-test-driven-development-with-visual-studio-2012/). Answer the following questions briefly and accurately in your own words:

- What are the advantages and disadvantages of writing unit tests?
- What is the difference between unit, integration and system tests?
- What is the lifecycle of TDD?

### Task 2

Implement a simple Calculator application in TDD manner. The application should enable the following computations:

- Multiplication of two floating numbers.
- Division of two floating numbers. If the second number value is 0, `double.NaN` (i.e. an _error code_) should be returned.

> ℹ️ **Info**
> A library that helps with TDD in C# is [xunit](https://www.nuget.org/packages/xunit) and its [runner for Visual Studio](https://www.nuget.org/packages/xunit.runner.visualstudio). You will also need [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk) package.

### Task 3

Inform yourself about [Mock Objects](http://en.wikipedia.org/wiki/Mock_object) and when they should be used. Extend the "**Calculator**" class from the "**Task 2**" to write the result of the computation to a file on a local hard drive. Use TDD and Mock Objects to simulate exceptional situations (_e.g., drive is not ready or file is locked_). In case of such situations, an appropriate IO exception should be thrown by both methods.

> ℹ️ **Info**
> A library that helps with creating Mock Objects in C# is [Moq](https://www.nuget.org/packages/Moq).
