using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                // Consuming ConcatenatorService
                var serviceLocation = "http://vsr-demo.informatik.tu-chemnitz.de/webservices/ConcatenatorService/ConcatenatorService.asmx";
                var ns = "http://tempuri.org/";
                var operationName = "Concatenate";
                var parameters = new Dictionary<string, object>()
                {
                    {"joinSymbol", ":"},
                    {"first", "bla"},
                    {"second", 5},
                    {"third", "?"},
                };
                var result = await SoapClient.Invoke(serviceLocation, ns, operationName, parameters);
                Console.WriteLine("Concatenator Result: " + result.ToString());

                // Consuming Another Service
                serviceLocation = "http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/Service.asmx";
                ns = "http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/";
                operationName = "Add";
                parameters = new Dictionary<string, object>()
                {
                    {"a", 10},
                    {"b", 15}
                };
                result = await SoapClient.Invoke(serviceLocation, ns, operationName, parameters);
                Console.WriteLine("Add Result: " + result.ToString());
            }).Wait();
        }
    }
}
