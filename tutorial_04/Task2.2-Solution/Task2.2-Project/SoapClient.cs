using System.Globalization;
using System.Security;
using System.Xml.Linq;

namespace SSE;

public static class SoapClient
{
    private const string Soap12Namespace = "http://www.w3.org/2003/05/soap-envelope";

    public static async Task<object> Invoke(string url, string ns, string operationName, Dictionary<string, object> parameters)
    {
        var soapAction = BuildSoapAction(ns, operationName);
        var content = BuildSoapEnvelope(ns, operationName, parameters);

        var headers = new Dictionary<string, string>
        {
            ["Content-Type"] = $"application/soap+xml; charset=utf-8; action=\"{soapAction}\"",
            ["Accept"] = "application/soap+xml, text/xml"
        };

        var response = await HttpRequest.Post(url, content, headers);
        var xml = XDocument.Parse(response.Content);

        ThrowIfFault(xml);
        return ParseResult(xml, operationName);
    }

    private static string BuildSoapAction(string ns, string operationName)
    {
        if (ns.EndsWith('/') || ns.EndsWith('#'))
        {
            return ns + operationName;
        }

        return ns + "/" + operationName;
    }

    private static string BuildSoapEnvelope(string ns, string operationName, Dictionary<string, object> parameters)
    {
        var parameterXml = string.Concat(parameters.Select(parameter =>
            $"<{parameter.Key}>{EscapeXml(FormatParameterValue(parameter.Value))}</{parameter.Key}>"));

        return $"""
        <?xml version="1.0" encoding="utf-8"?>
        <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                         xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                         xmlns:soap12="{Soap12Namespace}">
          <soap12:Body>
            <{operationName} xmlns="{ns}">{parameterXml}</{operationName}>
          </soap12:Body>
        </soap12:Envelope>
        """;
    }

    private static string FormatParameterValue(object value)
    {
        return value switch
        {
            int intValue => intValue.ToString(CultureInfo.InvariantCulture),
            string stringValue => stringValue,
            _ => throw new ArgumentException("Only string and integer parameters are supported.")
        };
    }

    private static string EscapeXml(string value)
    {
        return SecurityElement.Escape(value) ?? string.Empty;
    }

    private static void ThrowIfFault(XDocument xml)
    {
        var fault = xml.Descendants().FirstOrDefault(element => element.Name.LocalName == "Fault");
        if (fault is null)
        {
            return;
        }

        var reason = fault.Descendants().FirstOrDefault(element => element.Name.LocalName == "Text")?.Value
                     ?? fault.Value;

        throw new InvalidOperationException("SOAP fault: " + reason.Trim());
    }

    private static object ParseResult(XDocument xml, string operationName)
    {
        var body = xml.Descendants().FirstOrDefault(element => element.Name.LocalName == "Body")
                   ?? throw new FormatException("SOAP body not found.");

        var responseElement = body.Elements().FirstOrDefault(element =>
                                  element.Name.LocalName.Equals(operationName + "Response", StringComparison.OrdinalIgnoreCase))
                              ?? body.Elements().FirstOrDefault()
                              ?? throw new FormatException("SOAP response payload not found.");

        var resultElement = responseElement.Elements().FirstOrDefault(element =>
                                element.Name.LocalName.EndsWith("Result", StringComparison.OrdinalIgnoreCase))
                            ?? responseElement.Elements().FirstOrDefault()
                            ?? throw new FormatException("SOAP response does not contain a result value.");

        var rawValue = resultElement.Value.Trim();
        if (int.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
        {
            return intValue;
        }

        return rawValue;
    }
}