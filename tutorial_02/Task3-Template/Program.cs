namespace SSE
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var host = "127.0.0.1";
                var port = 13000;
                Server server = new Server(new Middleware());
                server.Start(host, port);

                Client client = new Client(new Middleware());

                await client.Print($"{host}:{port}", 2.25);
                Console.WriteLine();

                await client.Print($"{host}:{port}", "hello");
                Console.WriteLine();

                await client.Print($"{host}:{port}", new String[] { "a", "b", "c" });
                Console.WriteLine();

                Console.WriteLine("-------------");
                Console.WriteLine("Performing RPC...");
                Console.WriteLine("-------------");

                Console.WriteLine($"Concatenation Result: {await client.concat("a", "b", "c")}");

                Console.WriteLine($"Substring Result: {await client.substring("hello world", "6")}");

                //server.Stop();
            }).Wait();
        }
    }
}