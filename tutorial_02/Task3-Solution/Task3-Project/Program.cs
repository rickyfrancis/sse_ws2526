using SSE;

var host = "127.0.0.1";
var port = 13000;
var address = $"{host}:{port}";

var server = new Server(new Middleware());
server.Start(host, port);

var client = new Client(new Middleware(), address);

await client.Print(address, 2.25);
Console.WriteLine();

await client.Print(address, "hello");
Console.WriteLine();

await client.Print(address, ["a", "b", "c"]);
Console.WriteLine();

Console.WriteLine("-------------");
Console.WriteLine("Performing RPC...");
Console.WriteLine("-------------");

Console.WriteLine($"Concatenation Result: {await client.Concat("a", "b", "c")}");
Console.WriteLine($"Substring Result: {await client.Substring("hello world", "6")}");

server.Stop();
