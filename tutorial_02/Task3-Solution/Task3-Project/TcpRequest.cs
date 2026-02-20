using System.Net.Sockets;
using System.Text;

namespace SSE;

public static class TcpRequest
{
    public static async Task<string> Do(string ip, int port, string message)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(ip, port);

        using var reader = new StreamReader(client.GetStream(), Encoding.UTF8);
        using var writer = new StreamWriter(client.GetStream(), Encoding.UTF8);

        await writer.WriteAsync(message);
        await writer.FlushAsync();

        var buffer = new char[4096];
        _ = await reader.ReadAsync(buffer.AsMemory(0, buffer.Length));
        return new string(buffer);
    }
}
