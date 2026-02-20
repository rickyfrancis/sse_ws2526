namespace SSE;

public class HttpMessage
{
    public const string GET = "GET";
    public const string POST = "POST";

    public string? Method = string.Empty;
    public string? Host = string.Empty;
    public string? Resource = string.Empty;
    public Dictionary<string, string> Headers = new(StringComparer.OrdinalIgnoreCase);
    public string Content = string.Empty;
    public string? StatusCode = string.Empty;
    public string? StatusMessage = string.Empty;

    public HttpMessage(string method, string host, string resource, Dictionary<string, string>? headers, string content)
    {
        Method = method;
        Host = host;
        Resource = resource;
        Headers = headers ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        Content = content;
        StatusCode = null;
        StatusMessage = null;
    }

    public HttpMessage(string statusCode, string statusMessage, Dictionary<string, string>? headers, string content)
    {
        Method = null;
        Host = null;
        Resource = null;
        Headers = headers ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        Content = content;
        StatusCode = statusCode;
        StatusMessage = statusMessage;
    }

    public HttpMessage(string message)
    {
        var normalized = message.Replace("\r\n", "\n");
        var lines = normalized.Split('\n');
        var firstLine = lines.First();

        var parts = firstLine.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3)
        {
            throw new FormatException("Malformed HTTP message: " + firstLine);
        }

        if (parts[0].StartsWith("HTTP/", StringComparison.OrdinalIgnoreCase))
        {
            StatusCode = parts[1];
            StatusMessage = parts[2];
            Method = null;
            Resource = null;
            Host = null;
        }
        else
        {
            Method = parts[0];
            Resource = parts[1];
            StatusCode = null;
            StatusMessage = null;
        }

        Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line == string.Empty)
            {
                Content = string.Join("\n", lines.Skip(i + 1));
                break;
            }

            var colonAt = line.IndexOf(':');
            if (colonAt == -1)
            {
                throw new FormatException("Malformed header: " + line);
            }

            var name = line[..colonAt].Trim();
            var value = line[(colonAt + 1)..].Trim();
            if (name.Equals("Host", StringComparison.OrdinalIgnoreCase))
            {
                Host = value;
            }
            else
            {
                Headers[name] = value;
            }
        }
    }

    public override string ToString()
    {
        Headers["Content-Length"] = Content.Length.ToString();
        const string CrLf = "\r\n";

        string message;
        if (Method is not null)
        {
            message = $"{Method} {Resource} HTTP/1.1{CrLf}Host: {Host}{CrLf}";
        }
        else
        {
            message = $"HTTP/1.1 {StatusCode} {StatusMessage}{CrLf}";
        }

        foreach (var header in Headers)
        {
            message += $"{header.Key}: {header.Value}{CrLf}";
        }

        message += CrLf + Content;
        return message;
    }
}
