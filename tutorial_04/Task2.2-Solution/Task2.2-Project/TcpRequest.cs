using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SSE;

public static class TcpRequest
{
    public static async Task<string> Do(IPAddress ip, int port, string message)
    {
        using var client = new TcpClient(ip.AddressFamily);
        await client.ConnectAsync(ip, port);

        var utf8NoBom = new UTF8Encoding(false);
        using var reader = new StreamReader(client.GetStream(), utf8NoBom);
        using var writer = new StreamWriter(client.GetStream(), utf8NoBom);

        await writer.WriteAsync(message);
        await writer.FlushAsync();

        var buffer = new char[8192];
        var bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length);
        return new string(buffer, 0, bytesRead);
    }
}