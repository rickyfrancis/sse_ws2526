namespace Task3Project;

public class FileResultWriterTests
{
    [Fact]
    public void WriteResult_AppendsValueToFile()
    {
        var tempFilePath = Path.GetTempFileName();

        try
        {
            var writer = new FileResultWriter(tempFilePath);

            writer.WriteResult(12.5);

            var content = File.ReadAllText(tempFilePath);
            Assert.Contains("12.5", content);
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
}
