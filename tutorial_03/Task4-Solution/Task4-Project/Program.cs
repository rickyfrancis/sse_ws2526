using SSE;

var host = "127.0.0.1";
var port = 3000;

var server = new HttpServer();
Console.WriteLine($"Listening on http://{host}:{port}");
Console.WriteLine("Stop server with CTRL+C");

var serverTask = server.Start(host, port);

var done = new TaskCompletionSource<object?>();
Console.CancelKeyPress += (_, eventArgs) =>
{
	eventArgs.Cancel = true;
	server.Stop();
	done.TrySetResult(null);
};

await done.Task;
await serverTask;
