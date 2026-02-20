namespace Task1Solution;

public class ProxyTests
{
    private sealed class FakeCalculator : ICalculator
    {
        public int MultiplyCalls { get; private set; }
        public int DivideCalls { get; private set; }

        public double Multiply(double lhs, double rhs)
        {
            MultiplyCalls++;
            return lhs * rhs;
        }

        public double Divide(double lhs, double rhs)
        {
            DivideCalls++;
            return rhs == 0 ? double.NaN : lhs / rhs;
        }
    }

    [Fact]
    public void Multiply_ReturnsCachedResult_OnSecondSameCall()
    {
        var fakeCalculator = new FakeCalculator();
        var proxy = new Proxy(fakeCalculator);

        var first = proxy.Multiply(3, 2);
        var second = proxy.Multiply(3, 2);

        Assert.Equal(6, first);
        Assert.Equal(6, second);
        Assert.Equal(1, fakeCalculator.MultiplyCalls);
    }

    [Fact]
    public void Divide_ReturnsCachedResult_OnSecondSameCall()
    {
        var fakeCalculator = new FakeCalculator();
        var proxy = new Proxy(fakeCalculator);

        var first = proxy.Divide(6, 3);
        var second = proxy.Divide(6, 3);

        Assert.Equal(2, first);
        Assert.Equal(2, second);
        Assert.Equal(1, fakeCalculator.DivideCalls);
    }

    [Fact]
    public void Cache_EvictsOldest_WhenMoreThanTenCalculationsAreStored()
    {
        var fakeCalculator = new FakeCalculator();
        var proxy = new Proxy(fakeCalculator);

        for (var i = 1; i <= 10; i++)
        {
            proxy.Multiply(i, i + 1);
        }

        Assert.Equal(10, fakeCalculator.MultiplyCalls);

        proxy.Multiply(11, 12);
        Assert.Equal(11, fakeCalculator.MultiplyCalls);

        proxy.Multiply(1, 2);

        Assert.Equal(12, fakeCalculator.MultiplyCalls);
    }

    [Fact]
    public void Cache_KeyIncludesOperationType()
    {
        var fakeCalculator = new FakeCalculator();
        var proxy = new Proxy(fakeCalculator);

        proxy.Multiply(6, 3);
        proxy.Divide(6, 3);
        proxy.Multiply(6, 3);
        proxy.Divide(6, 3);

        Assert.Equal(1, fakeCalculator.MultiplyCalls);
        Assert.Equal(1, fakeCalculator.DivideCalls);
    }
}
