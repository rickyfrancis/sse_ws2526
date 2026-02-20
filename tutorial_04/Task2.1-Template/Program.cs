using System;
using System.Threading.Tasks;

namespace SSE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var serviceLocation = "http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/Service.asmx";
                var client = new AddServiceClient(serviceLocation);
                var answer = await client.Add(2,3);

                // display result and wait for user
                Console.WriteLine("Result: " + answer.ToString());
            }).Wait();
        }
    }
}
