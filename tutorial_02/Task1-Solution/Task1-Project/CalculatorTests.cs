namespace Task1Solution;

public class CalculatorTests
{
    [Fact]
    public void Multiply_ReturnsProduct()
    {
        var calculator = new Calculator();

        var result = calculator.Multiply(3, 2);

        Assert.Equal(6, result);
    }

    [Fact]
    public void Divide_ReturnsQuotient()
    {
        var calculator = new Calculator();

        var result = calculator.Divide(6, 3);

        Assert.Equal(2, result);
    }

    [Fact]
    public void Divide_ByZero_ReturnsNaN()
    {
        var calculator = new Calculator();

        var result = calculator.Divide(6, 0);

        Assert.True(double.IsNaN(result));
    }
}
