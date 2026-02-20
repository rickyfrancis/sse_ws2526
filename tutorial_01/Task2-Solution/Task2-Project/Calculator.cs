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
