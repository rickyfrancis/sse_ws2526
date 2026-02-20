using SSE;

namespace Task3Tests;

public class ServerRpcTests
{
    private readonly Server _server = new(new Middleware());

    [Fact]
    public void HandleRequest_RpcConcat_InvokesMethodUsingReflection()
    {
        var result = _server.HandleRequest(new RpcRequest("concat", ["a", "b", "c"]));

        Assert.Equal("abc", result);
    }

    [Fact]
    public void HandleRequest_RpcSubstring_InvokesMethodUsingReflection()
    {
        var result = _server.HandleRequest(new RpcRequest("substring", ["hello world", "6"]));

        Assert.Equal("world", result);
    }

    [Fact]
    public void HandleRequest_RpcUnknown_ReturnsRpcError()
    {
        var result = _server.HandleRequest(new RpcRequest("notExisting", ["x"]));

        Assert.StartsWith("RPC-ERROR:", result);
    }

    [Fact]
    public void HandleRequest_String_PrintPathStillWorks()
    {
        var result = _server.HandleRequest("hello");

        Assert.Equal("String printed", result);
    }
}
