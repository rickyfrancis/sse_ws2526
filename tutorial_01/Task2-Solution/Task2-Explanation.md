# Tutorial 01 – Task 2 (Calculator with TDD) – Exam Notes

## 1) What the task asks

From `tutorial_01.md`, Task 2 requires a calculator that supports:

1. Multiplication of two floating numbers
2. Division of two floating numbers
3. If divisor is `0`, return `double.NaN`

This is implemented in `tutorial_01/Task2-Solution/Task2-Project` with modern .NET tooling.

---

## 2) Tooling used (latest in this workspace)

- Target framework: `net10.0`
- Test framework: `xunit` `2.9.3`
- Test runner integration: `xunit.runner.visualstudio` `3.1.5`
- Test SDK: `Microsoft.NET.Test.Sdk` `18.0.1`

Files:

- `Task2-Solution.slnx` (solution container)
- `Task2-Project.csproj` (project/dependency config)

---

## 3) TDD lifecycle (exam style)

TDD uses a short loop called **Red → Green → Refactor**:

1. **Red**: write a failing test for one behavior
2. **Green**: write the minimum code to pass that test
3. **Refactor**: improve code structure while keeping tests green

How it maps here:

- Behavior 1 test: multiply returns product
- Behavior 2 test: divide returns quotient
- Behavior 3 test: divide-by-zero returns `NaN`
- Implementation then satisfies all tests

---

## 4) Code walkthrough

## `Calculator.cs`

```csharp
namespace Task2Project;

public class Calculator
{
    public double Multiply(double lhs, double rhs)
    {
        return lhs * rhs;
    }

    public double Divide(double lhs, double rhs)
    {
        return rhs == 0 ? double.NaN : lhs / rhs;
    }
}
```

Key points:

- `Multiply` is direct arithmetic
- `Divide` protects against invalid denominator with `double.NaN`

## `CalculatorTests.cs`

```csharp
namespace Task2Project;

public class CalculatorTests
{
    [Fact]
    public void Multiply_ReturnsProduct_ForTwoFloatingNumbers()
    {
        var calculator = new Calculator();
        var result = calculator.Multiply(3.5, 2.0);
        Assert.Equal(7.0, result);
    }

    [Fact]
    public void Divide_ReturnsQuotient_ForTwoFloatingNumbers()
    {
        var calculator = new Calculator();
        var result = calculator.Divide(7.5, 2.5);
        Assert.Equal(3.0, result);
    }

    [Fact]
    public void Divide_ReturnsNaN_WhenSecondNumberIsZero()
    {
        var calculator = new Calculator();
        var result = calculator.Divide(5.0, 0.0);
        Assert.True(double.IsNaN(result));
    }
}
```

Why `Assert.True(double.IsNaN(result))`?

- `NaN` has special comparison behavior; checking with `IsNaN` is safest and explicit.

---

## 5) Complexity and behavior

- `Multiply`: `O(1)` time, `O(1)` space
- `Divide`: `O(1)` time, `O(1)` space
- Error signaling rule: division by zero returns `double.NaN` (not exception in Task 2)

---

## 6) How to run (CLI)

From `tutorial_01/Task2-Solution`:

- `dotnet test .\Task2-Project\Task2-Project.csproj`

Result on this setup: 3 tests passed.

### Run calculator as a program

Task 2 now also includes a console runner project: `Task2-Cli`.

- Single operation mode:
  - `dotnet run --project .\Task2-Cli\Task2-Cli.csproj -- mul 3.5 2`
  - `dotnet run --project .\Task2-Cli\Task2-Cli.csproj -- div 7.5 2.5`

- Interactive mode:
  - `dotnet run --project .\Task2-Cli\Task2-Cli.csproj`
  - then enter commands like:
    - `mul 3 2`
    - `div 6 0`
    - `exit`

Notes:

- Use decimal dot format (e.g., `3.5`).
- `div` by zero prints `NaN` as required by Task 2.

---

## 7) Typical oral exam questions

### Q1: Why TDD for this task?

Because tests define expected behavior first and keep implementation correct during changes.

### Q2: Why return `double.NaN` instead of throwing?

The task explicitly defines `double.NaN` as the error code for divide-by-zero in Task 2.

### Q3: What is one benefit of this test design?

Each test verifies exactly one behavior, making failures easy to locate and fix.

---

## 8) One-sentence exam summary

This Task 2 solution follows TDD to implement a minimal calculator where multiply/divide work for floating numbers and divide-by-zero returns `double.NaN`, with tests proving each requirement.
