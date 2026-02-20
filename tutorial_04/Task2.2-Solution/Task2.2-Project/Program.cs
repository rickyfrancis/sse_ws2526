using SSE;

var concatenatorUrl = "http://vsr-demo.informatik.tu-chemnitz.de/webservices/ConcatenatorService/ConcatenatorService.asmx";
var concatenatorNs = "http://tempuri.org/";
var concatenateParameters = new Dictionary<string, object>
{
	["joinSymbol"] = ":",
	["first"] = "bla",
	["second"] = 5,
	["third"] = "?"
};

try
{
	var concatenatorResult = await SoapClient.Invoke(
		concatenatorUrl,
		concatenatorNs,
		"Concatenate",
		concatenateParameters);

	Console.WriteLine("Concatenator Result: " + concatenatorResult);
}
catch (Exception ex)
{
	Console.WriteLine("Concatenator request failed: " + ex.Message);
}

var addUrl = "http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/Service.asmx";
var addNs = "http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/";
var addParameters = new Dictionary<string, object>
{
	["a"] = 10,
	["b"] = 15
};

try
{
	var addResult = await SoapClient.Invoke(
		addUrl,
		addNs,
		"Add",
		addParameters);

	Console.WriteLine("Add Result: " + addResult);
}
catch (Exception ex)
{
	Console.WriteLine("Add request failed: " + ex.Message);
}
