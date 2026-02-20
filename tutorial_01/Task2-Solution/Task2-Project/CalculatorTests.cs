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
