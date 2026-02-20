using Task1Project;

if (args.Length >= 2)
{
	HandleCommand(args[0], string.Join(' ', args.Skip(1)));
	return;
}

Console.WriteLine("Task1 URL CLI");
Console.WriteLine("Commands:");
Console.WriteLine("  parse <url>");
Console.WriteLine("  encode <text>");
Console.WriteLine("  decode <text>");
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
		Console.WriteLine("Invalid input. Use: parse|encode|decode <value>");
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
				var url = new Url(value);
				Console.WriteLine($"Scheme:   {url.Scheme}");
				Console.WriteLine($"Host:     {url.Host}");
				Console.WriteLine($"Port:     {url.Port}");
				Console.WriteLine($"Path:     {url.Path}");
				Console.WriteLine($"Query:    {url.Query}");
				Console.WriteLine($"Fragment: {url.FragmentId}");
				Console.WriteLine($"ToString: {url}");
				break;

			case "encode":
				Console.WriteLine(Url.Encode(value));
				break;

			case "decode":
				Console.WriteLine(Url.Decode(value));
				break;

			default:
				Console.WriteLine("Unknown command. Use parse|encode|decode");
				break;
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error: {ex.Message}");
	}
}
