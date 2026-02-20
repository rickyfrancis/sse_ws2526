namespace SSE;

public class Middleware
{
    private Func<object, string>? _serverCallback;
    private Task? _serverTask;
    private CancellationTokenSource? _tokenSource;

    private object Demarshall(string line)
    {
        return RpcCodec.Demarshall(line);
    }

    private string Marshall(object input)
    {
        return RpcCodec.Marshall(input);
    }

    public async Task<string> SendObjectTo(string address, object input)
    {
        var parts = address.Split(':', 2);
        var ip = parts[0];
        var port = int.Parse(parts[1]);

        var payload = Marshall(input);
        Console.WriteLine($"\tMiddleware: Transferring payload '{payload}' to {ip}:{port}");

        var answer = await TcpRequest.Do(ip, port, payload);
        return answer.TrimEnd('\0');
    }

    public Task<string> CallFunction(string address, string name, string[] args)
    {
        var request = new RpcRequest(name, args);
        return SendObjectTo(address, request);
    }

    protected virtual string ProcessIncomingRequest(string line)
    {
        line = line.TrimEnd('\0');
        Console.WriteLine($"\tMiddleware: Received payload '{line}'");
        var request = Demarshall(line);
        return _serverCallback!(request);
    }

    public void StartServer(string ip, int port, Func<object, string> callback)
    {
        _serverCallback = callback;
        _tokenSource = new CancellationTokenSource();
        _serverTask = TcpServer.Start(ip, port, _tokenSource, ProcessIncomingRequest);
    }

    public void StopServer()
    {
        if (_tokenSource is null || _serverTask is null)
        {
            return;
        }

        _tokenSource.Cancel();
        _serverTask.Wait();
    }
}
