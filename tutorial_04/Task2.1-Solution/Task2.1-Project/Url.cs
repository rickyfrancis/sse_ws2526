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

    public Url(string urlStr)
    {
        var match = Regex.Match(urlStr, @"^(?:(?<scheme>[^:]+)://(?<host>[^/:?#]+)(?::(?<port>\d+))?)?(?<path>/[^?#]*)?(?:\?(?<query>[^#]*))?(?:#(?<fragmentid>.*))?$");
        if (!match.Success)
        {
            throw new FormatException("Could not parse URL: " + urlStr);
        }

        Scheme = match.Groups["scheme"].Value.ToLowerInvariant();
        Host = Decode(match.Groups["host"].Value);
        if (!string.IsNullOrEmpty(match.Groups["port"].Value))
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

        if (!string.IsNullOrEmpty(Path))
        {
            url += "/" + Encode(Path.TrimStart('/'));
        }

        if (!string.IsNullOrEmpty(Query))
        {
            var queryParts = Query.Split('&', StringSplitOptions.RemoveEmptyEntries);
            var encoded = new List<string>();
            foreach (var part in queryParts)
            {
                var nameValue = part.Split('=', 2);
                var name = nameValue[0];
                var value = nameValue.Length > 1 ? nameValue[1] : string.Empty;
                encoded.Add(Encode(name) + "=" + Encode(value));
            }

            url += "?" + string.Join("&", encoded);
        }

        if (!string.IsNullOrEmpty(FragmentId))
        {
            url += "#" + Encode(FragmentId);
        }

        return url;
    }

    public static string Encode(string s)
    {
        var result = string.Empty;
        foreach (var character in s)
        {
            if (ValidCharacters.Contains(character))
            {
                result += character;
            }
            else
            {
                result += "%" + Convert.ToByte(character).ToString("X2");
            }
        }

        return result;
    }

    public static string Decode(string s)
    {
        while (s.Contains('%'))
        {
            var pos = s.IndexOf('%');
            var b = byte.Parse(s.Substring(pos + 1, 2), System.Globalization.NumberStyles.HexNumber);
            s = s[..pos] + Convert.ToChar(b) + s[(pos + 3)..];
        }

        return s;
    }
}
