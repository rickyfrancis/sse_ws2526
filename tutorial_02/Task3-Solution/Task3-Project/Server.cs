using System.Reflection;

namespace SSE;

public class Server
{
    private readonly Middleware _middleware;

    public Server(Middleware middleware)
    {
        _middleware = middleware;
    }

    public string HandleRequest(object request)
    {
        if (request is RpcRequest rpcRequest)
        {
            return InvokeRpc(rpcRequest);
        }

        return PrintToConsole(request);
    }

    private string InvokeRpc(RpcRequest request)
    {
        var method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .FirstOrDefault(candidate =>
                candidate.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) &&
                candidate.ReturnType == typeof(string) &&
                candidate.GetParameters().All(parameter => parameter.ParameterType == typeof(string)) &&
                candidate.GetParameters().Length == request.Args.Length);

        if (method is null)
        {
            return $"RPC-ERROR: Unknown procedure '{request.Name}'";
        }

        try
        {
            var result = method.Invoke(this, request.Args.Cast<object>().ToArray());
            return result?.ToString() ?? string.Empty;
        }
        catch (TargetInvocationException invocationException)
        {
            return $"RPC-ERROR: {invocationException.InnerException?.Message ?? invocationException.Message}";
        }
    }

    private string PrintToConsole(object request)
    {
        if (request is double value)
        {
            Console.WriteLine("Server: Printing double: " + value.ToString("0#.##0"));
            return "Double printed";
        }

        if (request is string line)
        {
            Console.WriteLine($"Server: Printing string: <<{line}>> (length {line.Length})");
            return "String printed";
        }

        if (request is string[] values)
        {
            var output = $"Server: Printing string array: [{string.Join(" - ", values)}] (number of elements: {values.Length})";
            Console.WriteLine(output);
            return "String array printed";
        }

        throw new ArgumentException("Can not print the object " + request);
    }

    public void Start(string ip, int port)
    {
        _middleware.StartServer(ip, port, HandleRequest);
    }

    public void Stop()
    {
        _middleware.StopServer();
    }

    public string Concat(string arg1, string arg2, string arg3)
    {
        return arg1 + arg2 + arg3;
    }

    public string Substring(string s, string startingPosition)
    {
        return s.Substring(int.Parse(startingPosition));
    }
}
