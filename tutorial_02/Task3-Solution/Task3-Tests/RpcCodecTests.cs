using SSE;

namespace Task3Tests;

public class RpcCodecTests
{
    [Fact]
    public void MarshallDemarshall_Double_RoundTrips()
    {
        var payload = RpcCodec.Marshall(2.25d);

        var value = RpcCodec.Demarshall(payload);

        Assert.IsType<double>(value);
        Assert.Equal(2.25d, (double)value);
    }

    [Fact]
    public void MarshallDemarshall_String_RoundTrips()
    {
        var payload = RpcCodec.Marshall("hello");

        var value = RpcCodec.Demarshall(payload);

        Assert.IsType<string>(value);
        Assert.Equal("hello", (string)value);
    }

    [Fact]
    public void MarshallDemarshall_StringArray_RoundTrips()
    {
        var payload = RpcCodec.Marshall(new[] { "a", "b", "c" });

        var value = RpcCodec.Demarshall(payload);

        var array = Assert.IsType<string[]>(value);
        Assert.Equal(["a", "b", "c"], array);
    }

    [Fact]
    public void MarshallDemarshall_RpcRequest_RoundTrips()
    {
        var payload = RpcCodec.Marshall(new RpcRequest("concat", ["a", "b", "c"]));

        var value = RpcCodec.Demarshall(payload);

        var request = Assert.IsType<RpcRequest>(value);
        Assert.Equal("concat", request.Name);
        Assert.Equal(["a", "b", "c"], request.Args);
    }
}
