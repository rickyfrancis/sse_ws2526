namespace SSE;

public class HttpServer : TcpServer
{
    protected override string HandleRequest(string msg)
    {
        var request = new HttpMessage(msg);
        var answer = ReceiveRequest(request);

        Console.WriteLine("HTTP: Sending answer.");
        return answer.ToString();
    }

    protected virtual HttpMessage ReceiveRequest(HttpMessage request)
    {
        var requestUrl = new Url(request.Resource ?? "/");

        if (requestUrl.Path == "/" && string.Equals(request.Method, HttpMessage.GET, StringComparison.OrdinalIgnoreCase))
        {
            var cookies = request.GetCookies();
            var previousCount = cookies.TryGetValue("visit-count", out var value) && int.TryParse(value, out var parsed)
                ? parsed
                : 0;

            var currentCount = previousCount + 1;
            var suffix = currentCount switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };

            var response = new HttpMessage(
                "200",
                "OK",
                new Dictionary<string, string>
                {
                    ["Content-Type"] = "text/html; charset=utf-8"
                },
                $"<html><body>This is your {currentCount}{suffix} visit to this site.<hr /></body></html>");

            response.SetCookie("visit-count", currentCount.ToString());
            return response;
        }

        return new HttpMessage(
            "404",
            "Not Found",
            new Dictionary<string, string>
            {
                ["Content-Type"] = "text/html; charset=utf-8"
            },
            "<html><body>The requested file could not be found.</body></html>");
    }
}
