using System.Text.RegularExpressions;

namespace SSE;

public class Url
{
    private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$-_.~";

    public string Scheme = string.Empty;
    public string Host = string.Empty;
    public int Port = 80;
    public string Path = string.Empty;
    public string Query = string.Empty;
    public string FragmentId = string.Empty;

    public Url(string url)
    {
        var match = Regex.Match(url, @"
            ^(?:
            (?<scheme>[^:]*)
            :\/\/(?<host>[^:^\/^?^#]*)
            (?:\:(?<port>\d*))?
            )?
            (?<path>\/[^?^#]*)?
            (?:\?(?<query>[^#]*))?
            (?:\#(?<fragmentid>.*))?$
        ", RegexOptions.IgnorePatternWhitespace);

        if (!match.Success)
        {
            throw new FormatException("Could not parse URL: " + url);
        }

        Scheme = match.Groups["scheme"].Value.ToLowerInvariant();
        Host = Decode(match.Groups["host"].Value);
        if (match.Groups["port"].Value != string.Empty)
        {
            Port = int.Parse(match.Groups["port"].Value);
        }

        Path = Decode(match.Groups["path"].Value);
        Query = Decode(match.Groups["query"].Value);
        FragmentId = Decode(match.Groups["fragmentid"].Value);
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
        var url = Scheme + "://" + Host;
        if (Port != 80)
        {
            url += ":" + Port;
        }

        if (Path != string.Empty)
        {
            url += "/" + Encode(Path[1..]);
        }

        if (Query != string.Empty)
        {
            var queryParts = Query.Split('&');
            var encodedQuery = string.Empty;
            foreach (var queryPart in queryParts)
            {
                var nameValue = queryPart.Split('=');
                encodedQuery += "&" + Encode(nameValue[0]) + "=" + Encode(nameValue[1]);
            }

            url += "?" + encodedQuery[1..];
        }

        if (FragmentId != string.Empty)
        {
            url += "#" + Encode(FragmentId);
        }

        return url;
    }

    public static string Encode(string value)
    {
        var result = string.Empty;
        foreach (var character in value)
        {
            if (ValidCharacters.Contains(character))
            {
                result += character;
            }
            else
            {
                result += "%" + Convert.ToByte(character).ToString("X");
            }
        }

        return result;
    }

    public static string Decode(string value)
    {
        var result = value;
        while (result.Contains('%'))
        {
            var position = result.IndexOf('%');
            var current = byte.Parse(result.Substring(position + 1, 2), System.Globalization.NumberStyles.HexNumber);
            result = result[..position] + Convert.ToChar(current) + result[(position + 3)..];
        }

        return result;
    }
}