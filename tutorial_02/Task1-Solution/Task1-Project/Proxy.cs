using System.Collections.Generic;

namespace Task1Solution;

public class Proxy : ICalculator
{
    private enum OperationType
    {
        Multiplication,
        Division
    }

    private readonly record struct Calculation(OperationType Type, double Lhs, double Rhs, double Result);

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

    private void Cache(OperationType operationType, double lhs, double rhs, double result)
    {
        if (_cache.Count >= CacheSize)
        {
            _cache.Dequeue();
        }

        _cache.Enqueue(new Calculation(operationType, lhs, rhs, result));
    }
}
