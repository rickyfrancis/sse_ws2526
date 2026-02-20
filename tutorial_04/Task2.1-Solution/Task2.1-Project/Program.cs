using SSE;

var serviceLocation = args.Length > 0
	? args[0]
	: "http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/Service.asmx";

var a = args.Length > 1 && int.TryParse(args[1], out var aArg) ? aArg : 2;
var b = args.Length > 2 && int.TryParse(args[2], out var bArg) ? bArg : 3;

try
{
	var client = new AddServiceClient(serviceLocation);
	var answer = await client.Add(a, b);
	Console.WriteLine($"Result: {answer}");
}
catch (Exception ex)
{
	Console.WriteLine("SOAP call failed: " + ex.Message);
	Console.WriteLine("Tip: service is reachable only from university network / VPN.");
}
