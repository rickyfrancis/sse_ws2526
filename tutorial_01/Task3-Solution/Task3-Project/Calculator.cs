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
