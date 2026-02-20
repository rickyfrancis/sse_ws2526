namespace SSE;

public class Client
{
    private readonly Middleware _middleware;
    private readonly string _rpcAddress;

    public Client(Middleware middleware, string rpcAddress)
    {
        _middleware = middleware;
        _rpcAddress = rpcAddress;
    }

    public async Task Print(string address, double d)
    {
        Console.WriteLine("Client: Received response: " + await _middleware.SendObjectTo(address, d));
    }

    public async Task Print(string address, string line)
    {
        Console.WriteLine("Client: Received response: " + await _middleware.SendObjectTo(address, line));
    }

    public async Task Print(string address, string[] page)
    {
        Console.WriteLine("Client: Received response: " + await _middleware.SendObjectTo(address, page));
    }

    public Task<string> Concat(string arg1, string arg2, string arg3)
    {
        return _middleware.CallFunction(_rpcAddress, "concat", [arg1, arg2, arg3]);
    }

    public Task<string> Substring(string s, string startingPosition)
    {
        return _middleware.CallFunction(_rpcAddress, "substring", [s, startingPosition]);
    }
}
