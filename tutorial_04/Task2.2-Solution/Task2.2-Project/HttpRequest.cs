using System.Net;
using System.Net.Sockets;

namespace SSE;

public static class HttpRequest
{
    public static Task<HttpMessage> Get(string url)
    {
        return Request("GET", new Url(url), null, new Dictionary<string, string>());
    }

    public static Task<HttpMessage> Post(string url, string content, Dictionary<string, string> headers)
    {
        return Request("POST", new Url(url), content, headers);
    }

    private static async Task<HttpMessage> Request(string method, Url url, string? content, Dictionary<string, string> headers)
    {
        var addresses = await Dns.GetHostAddressesAsync(url.Host);
        var ipv4 = addresses.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork)
            ?? throw new ArgumentException("Cannot resolve IPv4 for host: " + url.Host);

        var resource = url.Path + (string.IsNullOrEmpty(url.Query) ? string.Empty : "?" + url.Query);
        var request = new HttpMessage(method, url.Host, resource, headers, content ?? string.Empty);

        return new HttpMessage(await TcpRequest.Do(ipv4, url.Port, request.ToString()));
    }
}