using System.Globalization;

namespace Task3Project;

public class FileResultWriter : IResultWriter
{
    private readonly string _filePath;

    public FileResultWriter(string filePath)
    {
        _filePath = filePath;
    }

    public void WriteResult(double result)
    {
        var line = string.Create(CultureInfo.InvariantCulture, $"{result}{Environment.NewLine}");
        File.AppendAllText(_filePath, line);
    }
}
