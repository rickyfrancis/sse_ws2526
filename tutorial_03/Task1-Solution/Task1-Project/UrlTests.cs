namespace Task1Project;

public class UrlTests
{
    [Fact]
    public void ParseAndToString_TemplateExample_Passes()
    {
        const string source = "http://www.tu-chemnitz.de:8080/ein%20test?my-name=my-value&a=1#id";

        var url = new Url(source);

        Assert.Equal("http", url.Scheme);
        Assert.Equal("www.tu-chemnitz.de", url.Host);
        Assert.Equal(8080, url.Port);
        Assert.Equal("/ein test", url.Path);
        Assert.Equal("my-name=my-value&a=1", url.Query);
        Assert.Equal("id", url.FragmentId);
        Assert.Equal(source, url.ToString());
    }

    [Fact]
    public void Encode_EncodesCharactersOutsideValidSet()
    {
        var encoded = Url.Encode("ein test/ä");

        Assert.Equal("ein%20test%2F%C3%A4", encoded);
    }

    [Fact]
    public void Decode_DecodesEscapedCharacters()
    {
        var decoded = Url.Decode("ein%20test%2F%C3%A4");

        Assert.Equal("ein test/ä", decoded);
    }

    [Fact]
    public void Parse_WithoutOptionalParts_UsesDefaults()
    {
        var url = new Url("http://example.com/path");

        Assert.Equal("http", url.Scheme);
        Assert.Equal("example.com", url.Host);
        Assert.Equal(80, url.Port);
        Assert.Equal("/path", url.Path);
        Assert.Equal(string.Empty, url.Query);
        Assert.Equal(string.Empty, url.FragmentId);
        Assert.Equal("http://example.com/path", url.ToString());
    }
}
