using SSE;

namespace Task3Tests;

public class HttpMessageTests
{
    [Fact]
    public void ParseResponseMessage_TemplateCase_Passes()
    {
        var msg = new HttpMessage("HTTP/1.1 200 OK\nContent-Type: text/html\nContent-Length: 12\n\nhello world\n");

        Assert.Equal("200", msg.StatusCode);
        Assert.Equal("OK", msg.StatusMessage);
        Assert.Equal("text/html", msg.Headers["Content-Type"]);
        Assert.Equal("12", msg.Headers["Content-Length"]);
        Assert.Equal("hello world\n", msg.Content);
    }

    [Fact]
    public void ParseRequestMessage_TemplateCase_Passes()
    {
        var msg = new HttpMessage("POST /test HTTP/1.1\nHost: example.org\nContent-Length: 5\n\nhallo");

        Assert.Equal("POST", msg.Method);
        Assert.Equal("/test", msg.Resource);
        Assert.Equal("example.org", msg.Host);
        Assert.Equal("5", msg.Headers["Content-Length"]);
        Assert.Equal("hallo", msg.Content);
    }

    [Fact]
    public void ResponseToString_RoundTrip_TemplateCase_Passes()
    {
        const string message = "HTTP/1.1 200 OK\nContent-Type: text/html\nContent-Length: 12\n\nhello world\n";

        var parsed = new HttpMessage(message);

        Assert.Equal(message, parsed.ToString());
    }

    [Fact]
    public void RequestToString_RoundTrip_TemplateCase_Passes()
    {
        const string message = "POST /test HTTP/1.1\nHost: example.org\nContent-Length: 5\n\nhallo";

        var parsed = new HttpMessage(message);

        Assert.Equal(message, parsed.ToString());
    }

    [Fact]
    public void BuildRequest_ThenParse_PreservesValues()
    {
        var headers = new Dictionary<string, string>
        {
            ["Host"] = "example.org",
            ["Content-Length"] = "5"
        };

        var msg = new HttpMessage(HttpMessage.POST, "example.org", "/test", headers, "hallo");
        var reparsed = new HttpMessage(msg.ToString());

        Assert.Equal(HttpMessage.POST, reparsed.Method);
        Assert.Equal("/test", reparsed.Resource);
        Assert.Equal("example.org", reparsed.Host);
        Assert.Equal("hallo", reparsed.Content);
    }
}
