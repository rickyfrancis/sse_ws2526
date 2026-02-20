using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SSE;

public static class TcpServer
{
    public static async Task Start(string ip, int port, CancellationTokenSource tokenSource, Func<string, string> handler)
    {
        var server = new TcpListener(IPAddress.Parse(ip), port);
        server.Start();

        var ct = tokenSource.Token;
        using (ct.Register(server.Stop))
        {
            while (true)
            {
                TcpClient client;
                try
                {
                    client = await server.AcceptTcpClientAsync();
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (SocketException) when (ct.IsCancellationRequested)
                {
                    return;
                }

                using var reader = new StreamReader(client.GetStream(), Encoding.UTF8);
                using var writer = new StreamWriter(client.GetStream(), Encoding.UTF8);

                var buffer = new char[4096];
                _ = await reader.ReadAsync(buffer.AsMemory(0, buffer.Length));
                var response = handler(new string(buffer));

                await writer.WriteAsync(response);
                await writer.FlushAsync();
            }
        }
    }
}
