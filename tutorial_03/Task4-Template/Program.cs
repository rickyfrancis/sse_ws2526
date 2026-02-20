using System;
using System.Threading.Tasks;

namespace SSE
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var host = "127.0.0.1";
                var port = 3000;

                var server = new HttpServer();

                Console.WriteLine($"Listening on http://{host}:{port}");
                Console.WriteLine("Stop server with CTRL+C");
                await server.Start(host, port);
            }).Wait();
        }
    }
}