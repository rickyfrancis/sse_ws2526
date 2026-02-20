using Moq;

namespace Task3Project;

public class CalculatorTests
{
    [Fact]
    public void Multiply_ReturnsProduct_AndWritesResult()
    {
        var writerMock = new Mock<IResultWriter>();
        var calculator = new Calculator(writerMock.Object);

        var result = calculator.Multiply(3.5, 2.0);

        Assert.Equal(7.0, result);
        writerMock.Verify(writer => writer.WriteResult(7.0), Times.Once);
    }

    [Fact]
    public void Divide_ReturnsQuotient_AndWritesResult()
    {
        var writerMock = new Mock<IResultWriter>();
        var calculator = new Calculator(writerMock.Object);

        var result = calculator.Divide(9.0, 3.0);

        Assert.Equal(3.0, result);
        writerMock.Verify(writer => writer.WriteResult(3.0), Times.Once);
    }

    [Fact]
    public void Divide_ByZero_ReturnsNaN_AndWritesResult()
    {
        var writerMock = new Mock<IResultWriter>();
        var calculator = new Calculator(writerMock.Object);

        var result = calculator.Divide(9.0, 0.0);

        Assert.True(double.IsNaN(result));
        writerMock.Verify(writer => writer.WriteResult(It.Is<double>(value => double.IsNaN(value))), Times.Once);
    }

    [Fact]
    public void Multiply_WhenWriterThrowsIOException_ThrowsIOException()
    {
        var writerMock = new Mock<IResultWriter>();
        writerMock
            .Setup(writer => writer.WriteResult(It.IsAny<double>()))
            .Throws(new IOException("Drive not ready"));

        var calculator = new Calculator(writerMock.Object);

        Assert.Throws<IOException>(() => calculator.Multiply(3.0, 2.0));
    }

    [Fact]
    public void Divide_WhenWriterThrowsIOException_ThrowsIOException()
    {
        var writerMock = new Mock<IResultWriter>();
        writerMock
            .Setup(writer => writer.WriteResult(It.IsAny<double>()))
            .Throws(new IOException("File is locked"));

        var calculator = new Calculator(writerMock.Object);

        Assert.Throws<IOException>(() => calculator.Divide(6.0, 3.0));
    }
}
