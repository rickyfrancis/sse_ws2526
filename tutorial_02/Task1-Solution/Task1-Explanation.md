# Tutorial 02 – Task 1 (Proxy Pattern) – Exam Notes

## 0) Proxy Pattern Theory ("Get informed about the Proxy software design pattern")

### Exam-ready definition

The **Proxy pattern** is a structural design pattern where a proxy object has the same interface as a real object, but controls access to it.

### Why we use it

- Add behavior **before/after** calling the real object without changing client code.
- Typical reasons: caching, access control, lazy loading, logging, remote access.

### Standard participants

- **Subject**: shared interface (`ICalculator`)
- **RealSubject**: actual implementation (`Calculator`)
- **Proxy**: wrapper with same interface (`Proxy`)
- **Client**: depends only on Subject and can use RealSubject or Proxy transparently

### Common proxy variants (know in exam)

- **Virtual Proxy**: lazy initialization of expensive objects
- **Remote Proxy**: local stand-in for remote service
- **Protection Proxy**: authorization/access checks
- **Caching Proxy**: stores results to avoid repeated expensive work

### One-line exam pitch for this task

"This task implements a **caching proxy** for a calculator: same interface, same client usage, but with transparent result reuse for repeated calls."

---

## 1) What the task asks

From `tutorial_02.md`, Task 1 requires:

1. Use the **Proxy** design pattern for the calculator.
2. Cache the **last 10 calculations**.
3. If a new request has the same **operation + operands**, return the cached result.

This implementation satisfies that requirement in `tutorial_02/Task1-Solution/Task1-Project`.

---

## 2) Files and roles (updated to Task1-Solution)

### `ICalculator.cs`

Defines the shared contract.

```csharp
namespace Task1Solution;

public interface ICalculator
{
    double Multiply(double lhs, double rhs);
    double Divide(double lhs, double rhs);
}
```

Why important: both `Calculator` and `Proxy` are interchangeable for clients.

### `Calculator.cs`

Real implementation (no caching).

```csharp
namespace Task1Solution;

public class Calculator : ICalculator
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

### `Proxy.cs`

Proxy implementation with caching and delegation.

```csharp
public class Proxy : ICalculator
{
    private const int CacheSize = 10;
    private readonly Queue<Calculation> _cache = new(CacheSize);
    private readonly ICalculator _calculator;

    public Proxy()
        : this(new Calculator())
    {
    }

    public Proxy(ICalculator calculator)
    {
        _calculator = calculator;
    }
}
```

### `CalculatorTests.cs`

Validates multiply, divide, divide-by-zero behavior of the real object.

### `ProxyTests.cs`

Validates cache hit behavior, cache key semantics, and eviction after 10 entries.

---

## 3) Proxy internals (important for oral exam)

### Data structures in `Proxy`

- `OperationType` enum: distinguishes multiply vs divide
- `Calculation` record struct: cache row
- `_cache` queue: fixed-size FIFO cache
- `_calculator`: wrapped real object

```csharp
private enum OperationType
{
    Multiplication,
    Division
}

private readonly record struct Calculation(OperationType Type, double Lhs, double Rhs, double Result);
```

### Cache lookup: `FromCache(...)`

```csharp
private double? FromCache(OperationType operationType, double lhs, double rhs)
{
    foreach (var calculation in _cache)
    {
        if (calculation.Type == operationType && calculation.Lhs == lhs && calculation.Rhs == rhs)
        {
            return calculation.Result;
        }
    }

    return null;
}
```

### Cache insertion + eviction: `Cache(...)`

```csharp
private void Cache(OperationType operationType, double lhs, double rhs, double result)
{
    if (_cache.Count >= CacheSize)
    {
        _cache.Dequeue();
    }

    _cache.Enqueue(new Calculation(operationType, lhs, rhs, result));
}
```

This is FIFO eviction: when the 11th entry is added, the oldest one is removed.

---

## 4) Method flow

### `Multiply(lhs, rhs)`

```csharp
public double Multiply(double lhs, double rhs)
{
    var fromCache = FromCache(OperationType.Multiplication, lhs, rhs);
    if (fromCache.HasValue)
    {
        return fromCache.Value;
    }

    var result = _calculator.Multiply(lhs, rhs);
    Cache(OperationType.Multiplication, lhs, rhs, result);
    return result;
}
```

### `Divide(lhs, rhs)`

```csharp
public double Divide(double lhs, double rhs)
{
    var fromCache = FromCache(OperationType.Division, lhs, rhs);
    if (fromCache.HasValue)
    {
        return fromCache.Value;
    }

    var result = _calculator.Divide(lhs, rhs);
    Cache(OperationType.Division, lhs, rhs, result);
    return result;
}
```

Pattern summary: **check cache → delegate on miss → cache result**.

---

## 5) How tests prove correctness

### Real calculator tests (`CalculatorTests.cs`)

- `Multiply_ReturnsProduct`
- `Divide_ReturnsQuotient`
- `Divide_ByZero_ReturnsNaN`

### Proxy tests (`ProxyTests.cs`)

- `Multiply_ReturnsCachedResult_OnSecondSameCall`
- `Divide_ReturnsCachedResult_OnSecondSameCall`
- `Cache_EvictsOldest_WhenMoreThanTenCalculationsAreStored`
- `Cache_KeyIncludesOperationType`

These tests verify both functional correctness and proxy-specific behavior.

---

## 6) Complexity and edge cases

- Lookup cost: `O(n)` with `n <= 10` (small bounded linear search)
- Insert/evict: `O(1)`
- Memory: max 10 entries

Edge cases:

- Uses exact `double` equality for cache key
- Divide-by-zero (`NaN`) is also cacheable
- Not thread-safe by design (acceptable for tutorial scope)

---

## 7) One-sentence exam summary

This Task 1 solution is a **caching proxy**: `Proxy` implements the same `ICalculator` interface as `Calculator`, intercepts calls, reuses cached results for matching operation+operands, and keeps only the last 10 calculations.

---

## 8) How to run as a program (CLI)

Task 1 now also includes a console runner project: `Task1-Cli`.

From `tutorial_02/Task1-Solution`:

- Run tests:
  - `dotnet test .\Task1-Project\Task1-Solution.csproj`

- Run a single calculator operation:
  - `dotnet run --project .\Task1-Cli\Task1-Cli.csproj -- mul 3 2`
  - `dotnet run --project .\Task1-Cli\Task1-Cli.csproj -- div 7.5 2.5`

- Run interactive mode:
  - `dotnet run --project .\Task1-Cli\Task1-Cli.csproj`
  - then enter commands like:
    - `mul 3 2`
    - `div 6 0`
    - `exit`

Notes:

- CLI uses the `Proxy` implementation, so repeated operation+operand pairs are served from cache.
- Use decimal dot format (e.g., `3.5`).
