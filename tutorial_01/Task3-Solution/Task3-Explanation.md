# Tutorial 01 – Task 3 (TDD + Mock Objects + IO Exceptions) – Exam Notes

## 0) Mock Objects Theory (exam style)

### Exam-ready definition

A **Mock Object** is a test double that simulates a dependency and allows us to verify interactions (e.g., whether a method was called and with which arguments).

### Why use mock objects

- Isolate the unit under test from external systems (file system, DB, network).
- Make tests deterministic and fast.
- Simulate rare/error conditions that are hard to reproduce in real environments.
- Verify collaboration behavior ("was write called?").

### Stub vs Mock (short distinction)

- **Stub**: returns predefined data.
- **Mock**: additionally verifies interaction expectations.

### Why Moq here

Task 3 asks to simulate IO exceptions (e.g., drive not ready / file locked). With Moq, we can make the writer dependency throw `IOException` on demand and assert that `Calculator` propagates it.

---

## 1) What Task 3 asks

From `tutorial_01.md`, Task 3 requires:

1. Extend Task 2 `Calculator` to write computation results to local file.
2. Use TDD and Mock Objects.
3. Simulate exceptional IO situations.
4. In such situations, both methods should throw an appropriate IO exception.

This is implemented in `tutorial_01/Task3-Solution/Task3-Project`.

---

## 2) Architecture used in solution

To make file writing testable, the design separates concerns:

- `Calculator` does computation and delegates persistence.
- `IResultWriter` abstracts "write result" operation.
- `FileResultWriter` is the real file-system implementation.

This enables dependency injection and mocking.

---

## 3) Code walkthrough

### `IResultWriter.cs`

```csharp
namespace Task3Project;

public interface IResultWriter
{
    void WriteResult(double result);
}
```

### `Calculator.cs`

```csharp
namespace Task3Project;

public class Calculator
{
    private readonly IResultWriter _resultWriter;

    public Calculator(IResultWriter resultWriter)
    {
        _resultWriter = resultWriter;
    }

    public double Multiply(double lhs, double rhs)
    {
        var result = lhs * rhs;
        _resultWriter.WriteResult(result);
        return result;
    }

    public double Divide(double lhs, double rhs)
    {
        var result = rhs == 0 ? double.NaN : lhs / rhs;
        _resultWriter.WriteResult(result);
        return result;
    }
}
```

Important exam points:

- Both methods compute result and then write it.
- If writer throws `IOException`, the exception is not swallowed, so caller receives it (as required).

### `FileResultWriter.cs`

```csharp
using System.Globalization;

namespace Task3Project;

public class FileResultWriter : IResultWriter
{
    private readonly string _filePath;

    public FileResultWriter(string filePath)
    {
        _filePath = filePath;
    }

    public void WriteResult(double result)
    {
        var line = string.Create(CultureInfo.InvariantCulture, $"{result}{Environment.NewLine}");
        File.AppendAllText(_filePath, line);
    }
}
```

Why `InvariantCulture`:

- Keeps decimal format stable (`12.5`) regardless of OS locale.

---

## 4) TDD proof through tests

### Behavior tests and interaction verification (`CalculatorTests.cs`)

- `Multiply_ReturnsProduct_AndWritesResult`
- `Divide_ReturnsQuotient_AndWritesResult`
- `Divide_ByZero_ReturnsNaN_AndWritesResult`

These validate:

- Correct arithmetic behavior.
- Writer is called exactly once with expected value.

### Exception simulation with Mock Objects

- `Multiply_WhenWriterThrowsIOException_ThrowsIOException`
- `Divide_WhenWriterThrowsIOException_ThrowsIOException`

Pattern used:

```csharp
writerMock
    .Setup(writer => writer.WriteResult(It.IsAny<double>()))
    .Throws(new IOException("Drive not ready"));
```

This simulates IO failures without touching actual faulty drives/files.

### Real writer integration test (`FileResultWriterTests.cs`)

- `WriteResult_AppendsValueToFile`

Checks that real file writer appends output to disk.

---

## 5) Requirement-to-test mapping

- "Write result to local hard drive" → `FileResultWriterTests.WriteResult_AppendsValueToFile`
- "Use Mock Objects" → all `CalculatorTests` that use `Mock<IResultWriter>`
- "Simulate exceptional situations" → tests where mock throws `IOException`
- "Both methods should throw IO exception" → two exception tests (multiply + divide)

---

## 6) Complexity and edge cases

- `Multiply` and `Divide`: `O(1)` compute cost.
- File writing dominates runtime (`File.AppendAllText`).
- `Divide` with zero still returns `double.NaN` and writes that value.
- IO failures are propagated to caller as `IOException`.

---

## 7) How to run

From `tutorial_01/Task3-Solution`:

- `dotnet test .\Task3-Project\Task3-Project.csproj`

Current status on this setup: all tests pass.

### Run calculator as a program

Task 3 also includes a console runner project: `Task3-Cli`.

- Single operation mode (with optional output file path as 4th arg):
  - `dotnet run --project .\Task3-Cli\Task3-Cli.csproj -- mul 4 2 .\results.txt`
  - `dotnet run --project .\Task3-Cli\Task3-Cli.csproj -- div 9 3 .\results.txt`

- Interactive mode:
  - `dotnet run --project .\Task3-Cli\Task3-Cli.csproj`
  - then enter commands like:
    - `mul 3 2`
    - `div 6 0`
    - `exit`

Notes:

- Each operation writes the numeric result to the output file (default: `results.txt`).
- Use decimal dot format for input values (e.g., `3.5`).

---

## 8) Typical oral exam questions

### Q1: Why not call `File.AppendAllText` directly inside `Calculator`?

Because that tightly couples business logic to IO, making tests slow and hard to control. Abstraction (`IResultWriter`) enables unit testing with mocks.

### Q2: How do mocks help with exceptional cases?

Mocks can be configured to throw exceptions deterministically, so tests can verify error handling paths reliably.

### Q3: Is this still TDD-compliant?

Yes. Tests describe required behavior (including failures), implementation is then shaped to satisfy them.

---

## 9) One-sentence exam summary

Task 3 extends the calculator with file persistence via dependency injection and uses Moq-based tests to prove both normal behavior and required IO-exception propagation for `Multiply` and `Divide`.
