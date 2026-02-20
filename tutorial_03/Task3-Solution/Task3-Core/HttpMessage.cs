namespace SSE;

public class HttpMessage
{
    public const string GET = "GET";
    public const string POST = "POST";
    public const string PUT = "PUT";
    public const string DELETE = "DELETE";
    public const string HEAD = "HEAD";
    public const string OPTIONS = "OPTIONS";
    public const string TRACE = "TRACE";

    public string? Method = string.Empty;
    public string? Host = string.Empty;
    public string? Resource = string.Empty;
    public Dictionary<string, string> Headers = new();
    public string Content = string.Empty;
    public string? StatusCode = string.Empty;
    public string? StatusMessage = string.Empty;

    public HttpMessage(string method, string host, string resource, Dictionary<string, string>? headers, string content)
    {
        Method = method;
        Host = host;
        Resource = resource;
        Headers = headers ?? new Dictionary<string, string>();
        Content = content;
        StatusCode = null;
        StatusMessage = null;
    }

    public HttpMessage(string statusCode, string statusMessage, Dictionary<string, string>? headers, string content)
    {
        Method = null;
        Host = null;
        Resource = null;
        Headers = headers ?? new Dictionary<string, string>();
        Content = content;
        StatusCode = statusCode;
        StatusMessage = statusMessage;
    }

    public HttpMessage(string message)
    {
        Parse(message);
    }

    public override string ToString()
    {
        var lines = new List<string>();

        if (IsRequest)
        {
            lines.Add($"{Method} {Resource} HTTP/1.1");
        }
        else
        {
            lines.Add($"HTTP/1.1 {StatusCode} {StatusMessage}");
        }

        foreach (var header in Headers)
        {
            lines.Add($"{header.Key}: {header.Value}");
        }

        var head = string.Join("\n", lines);
        return head + "\n\n" + Content;
    }

    public bool IsRequest => Method is not null;

    private void Parse(string message)
    {
        message = message.Replace("\r\n", "\n");

        var separator = "\n\n";
        var separatorIndex = message.IndexOf(separator, StringComparison.Ordinal);

        string head;
        if (separatorIndex >= 0)
        {
            head = message[..separatorIndex];
            Content = message[(separatorIndex + separator.Length)..];
        }
        else
        {
            head = message;
            Content = string.Empty;
        }

        var lines = head.Split('\n');
        if (lines.Length == 0)
        {
            throw new FormatException("HTTP message is empty.");
        }

        ParseStartLine(lines[0]);

        Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var colonIndex = line.IndexOf(':');
            if (colonIndex <= 0)
            {
                throw new FormatException($"Invalid header line: {line}");
            }

            var name = line[..colonIndex].Trim();
            var value = line[(colonIndex + 1)..].TrimStart();
            Headers[name] = value;
        }

        if (IsRequest)
        {
            Host = Headers.TryGetValue("Host", out var hostHeader) ? hostHeader : string.Empty;
            StatusCode = null;
            StatusMessage = null;
        }
        else
        {
            Method = null;
            Resource = null;
            Host = null;
        }
    }

    private void ParseStartLine(string startLine)
    {
        if (startLine.StartsWith("HTTP/", StringComparison.OrdinalIgnoreCase))
        {
            var firstSpace = startLine.IndexOf(' ');
            var secondSpace = firstSpace < 0 ? -1 : startLine.IndexOf(' ', firstSpace + 1);
            if (firstSpace < 0 || secondSpace < 0)
            {
                throw new FormatException("Invalid HTTP response start line.");
            }

            StatusCode = startLine[(firstSpace + 1)..secondSpace];
            StatusMessage = startLine[(secondSpace + 1)..];
            Method = null;
            Resource = null;
            Host = null;
            return;
        }

        var parts = startLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3)
        {
            throw new FormatException("Invalid HTTP request start line.");
        }

        Method = parts[0];
        Resource = parts[1];
        StatusCode = null;
        StatusMessage = null;
    }
}
