using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;

namespace SSE;

public class AddServiceClient
{
    private readonly string _serviceLocation;

    public AddServiceClient(string serviceLocation)
    {
        _serviceLocation = serviceLocation;
    }

    public async Task<int> Add(int a, int b)
    {
        const string serviceNamespace = "http://tempuri.org/";
        const string soapNamespace = "http://www.w3.org/2003/05/soap-envelope";

        var content = $"""
        <?xml version="1.0" encoding="utf-8"?>
        <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                         xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                         xmlns:soap12="{soapNamespace}">
          <soap12:Body>
            <Add xmlns="{serviceNamespace}">
              <intA>{a}</intA>
              <intB>{b}</intB>
            </Add>
          </soap12:Body>
        </soap12:Envelope>
        """;

        var headers = new Dictionary<string, string>
        {
            ["Content-Type"] = "application/soap+xml; charset=utf-8; action=\"http://tempuri.org/Add\"",
            ["Accept"] = "application/soap+xml, text/xml"
        };

        var answer = await HttpRequest.Post(_serviceLocation, content, headers);

        var xml = XDocument.Parse(answer.Content);
        var manager = new XmlNamespaceManager(new NameTable());
        manager.AddNamespace("soap", soapNamespace);
        manager.AddNamespace("ns", serviceNamespace);

        var valueNode = xml.XPathSelectElement("//ns:AddResponse/ns:AddResult", manager)
                ?? xml.XPathSelectElement("//*[local-name()='AddResult']")
                ?? throw new FormatException("Could not parse AddResult from SOAP response.");

        return int.Parse(valueNode.Value);
    }
}
