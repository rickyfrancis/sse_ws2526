using SSE;

if (args.Length >= 1)
{
	HandleCommand(args[0], args.Length > 1 ? string.Join(' ', args.Skip(1)) : string.Empty);
	return;
}

Console.WriteLine("Task3 HTTP Message CLI");
Console.WriteLine("Commands:");
Console.WriteLine("  parse <raw_http_message>");
Console.WriteLine("  sample-request");
Console.WriteLine("  sample-response");
Console.WriteLine("  exit");

while (true)
{
	Console.Write("> ");
	var input = Console.ReadLine();
	if (string.IsNullOrWhiteSpace(input))
	{
		continue;
	}

	if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
	{
		break;
	}

	var splitIndex = input.IndexOf(' ');
	if (splitIndex < 0)
	{
		HandleCommand(input.Trim(), string.Empty);
		continue;
	}

	var command = input[..splitIndex];
	var value = input[(splitIndex + 1)..];
	HandleCommand(command, value);
}

return;

static void HandleCommand(string command, string value)
{
	try
	{
		switch (command.ToLowerInvariant())
		{
			case "parse":
				ParseAndPrint(value.Replace("\\n", "\n"));
				break;

			case "sample-request":
				var request = new HttpMessage(
					HttpMessage.POST,
					"example.org",
					"/test",
					new Dictionary<string, string>
					{
						["Host"] = "example.org",
						["Content-Length"] = "5"
					},
					"hallo");
				Console.WriteLine(request.ToString());
				break;

			case "sample-response":
				var response = new HttpMessage(
					"200",
					"OK",
					new Dictionary<string, string>
					{
						["Content-Type"] = "text/html",
						["Content-Length"] = "12"
					},
					"hello world\n");
				Console.WriteLine(response.ToString());
				break;

			default:
				Console.WriteLine("Unknown command. Use parse|sample-request|sample-response");
				break;
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error: {ex.Message}");
	}
}

static void ParseAndPrint(string raw)
{
	var message = new HttpMessage(raw);

	Console.WriteLine(message.IsRequest ? "Type: Request" : "Type: Response");

	if (message.IsRequest)
	{
		Console.WriteLine($"Method:   {message.Method}");
		Console.WriteLine($"Resource: {message.Resource}");
		Console.WriteLine($"Host:     {message.Host}");
	}
	else
	{
		Console.WriteLine($"Status:   {message.StatusCode} {message.StatusMessage}");
	}

	Console.WriteLine("Headers:");
	foreach (var header in message.Headers)
	{
		Console.WriteLine($"  {header.Key}: {header.Value}");
	}

	Console.WriteLine($"Content:  {message.Content}");
}
