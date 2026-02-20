using System.Text;
using System.Text.RegularExpressions;

namespace Task1Project;

public class Url
{
    private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$-_.~";

    public string Scheme = string.Empty;
    public string Host = string.Empty;
    public int Port = 80;
    public string Path = string.Empty;
    public string Query = string.Empty;
    public string FragmentId = string.Empty;

    public Url(string urlStr)
    {
        var match = Regex.Match(
            urlStr,
            @"^(?<scheme>[a-zA-Z][a-zA-Z0-9+\-.]*)://(?<host>[^/:?#]+)(?::(?<port>\d+))?(?<path>/[^?#]*)?(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?$",
            RegexOptions.Compiled);

        if (!match.Success)
        {
            throw new FormatException("Could not parse URL: " + urlStr);
        }

        Scheme = match.Groups["scheme"].Value;
        Host = match.Groups["host"].Value;

        if (match.Groups["port"].Success)
        {
            Port = int.Parse(match.Groups["port"].Value);
        }

        Path = match.Groups["path"].Success ? Decode(match.Groups["path"].Value) : string.Empty;
        Query = match.Groups["query"].Success ? Decode(match.Groups["query"].Value) : string.Empty;
        FragmentId = match.Groups["fragment"].Success ? Decode(match.Groups["fragment"].Value) : string.Empty;
    }

    public Url(string scheme, string host, int port, string path, string query, string fragmentId)
    {
        Scheme = scheme;
        Host = host;
        Port = port;
        Path = path;
        Query = query;
        FragmentId = fragmentId;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append(Scheme);
        builder.Append("://");
        builder.Append(Host);

        if (Port != 80)
        {
            builder.Append(':');
            builder.Append(Port);
        }

        builder.Append(EncodePreserving(Path, "/"));

        if (!string.IsNullOrEmpty(Query))
        {
            builder.Append('?');
            builder.Append(EncodePreserving(Query, "&="));
        }

        if (!string.IsNullOrEmpty(FragmentId))
        {
            builder.Append('#');
            builder.Append(Encode(FragmentId));
        }

        return builder.ToString();
    }

    public static string Encode(string s)
    {
        var builder = new StringBuilder();

        foreach (var character in s)
        {
            if (ValidCharacters.IndexOf(character) >= 0)
            {
                builder.Append(character);
                continue;
            }

            var bytes = Encoding.UTF8.GetBytes(character.ToString());
            foreach (var b in bytes)
            {
                builder.Append('%');
                builder.Append(b.ToString("X2"));
            }
        }

        return builder.ToString();
    }

    public static string Decode(string s)
    {
        var bytes = new List<byte>();
        var builder = new StringBuilder();

        for (var i = 0; i < s.Length; i++)
        {
            if (s[i] == '%' && i + 2 < s.Length &&
                IsHexDigit(s[i + 1]) &&
                IsHexDigit(s[i + 2]))
            {
                bytes.Add(Convert.ToByte(s.Substring(i + 1, 2), 16));
                i += 2;
                continue;
            }

            FlushBytes(builder, bytes);
            builder.Append(s[i]);
        }

        FlushBytes(builder, bytes);
        return builder.ToString();
    }

    private static string EncodePreserving(string value, string preserve)
    {
        var builder = new StringBuilder();

        foreach (var character in value)
        {
            if (preserve.IndexOf(character) >= 0)
            {
                builder.Append(character);
            }
            else
            {
                builder.Append(Encode(character.ToString()));
            }
        }

        return builder.ToString();
    }

    private static bool IsHexDigit(char character)
    {
        return (character >= '0' && character <= '9') ||
               (character >= 'A' && character <= 'F') ||
               (character >= 'a' && character <= 'f');
    }

    private static void FlushBytes(StringBuilder builder, List<byte> bytes)
    {
        if (bytes.Count == 0)
        {
            return;
        }

        builder.Append(Encoding.UTF8.GetString(bytes.ToArray()));
        bytes.Clear();
    }
}
