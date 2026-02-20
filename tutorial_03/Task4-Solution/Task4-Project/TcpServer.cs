using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SSE;

public class TcpServer
{
    private CancellationTokenSource? _tokenSource;
    private Task? _serverTask;

    protected virtual string HandleRequest(string msg)
    {
        return string.Empty;
    }

    public Task Start(string ip, int port)
    {
        _tokenSource = new CancellationTokenSource();
        _serverTask = Start(ip, port, _tokenSource);
        return _serverTask;
    }

    public void Stop()
    {
        if (_tokenSource is null || _serverTask is null)
        {
            return;
        }

        _tokenSource.Cancel();
        _serverTask.Wait();
    }

    private async Task Start(string ip, int port, CancellationTokenSource tokenSource)
    {
        var utf8NoBom = new UTF8Encoding(false);
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

                using var reader = new StreamReader(client.GetStream(), utf8NoBom);
                using var writer = new StreamWriter(client.GetStream(), utf8NoBom);

                var buffer = new char[4096];
                var bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length);
                var response = HandleRequest(new string(buffer, 0, bytesRead));

                await writer.WriteAsync(response);
                await writer.FlushAsync();
            }
        }
    }
}
