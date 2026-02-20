using System.Globalization;
using Task3Project;

var outputPath = args.Length >= 4 ? args[3] : "results.txt";
var calculator = new Calculator(new FileResultWriter(outputPath));

if (args.Length is 3 or 4)
{
	RunSingleOperation(args[0], args[1], args[2]);
	return;
}

Console.WriteLine("Task3 Calculator CLI");
Console.WriteLine($"Output file: {Path.GetFullPath(outputPath)}");
Console.WriteLine("Commands:");
Console.WriteLine("  mul <a> <b>");
Console.WriteLine("  div <a> <b>");
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

	var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
	if (parts.Length != 3)
	{
		Console.WriteLine("Invalid input. Use: mul <a> <b> or div <a> <b>");
		continue;
	}

	RunSingleOperation(parts[0], parts[1], parts[2]);
}

void RunSingleOperation(string op, string lhsRaw, string rhsRaw)
{
	if (!double.TryParse(lhsRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out var lhs) ||
		!double.TryParse(rhsRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out var rhs))
	{
		Console.WriteLine("Invalid number. Use decimal dot format, e.g. 3.5");
		return;
	}

	try
	{
		double result;
		if (op.Equals("mul", StringComparison.OrdinalIgnoreCase))
		{
			result = calculator.Multiply(lhs, rhs);
		}
		else if (op.Equals("div", StringComparison.OrdinalIgnoreCase))
		{
			result = calculator.Divide(lhs, rhs);
		}
		else
		{
			Console.WriteLine("Unknown operation. Use mul or div.");
			return;
		}

		Console.WriteLine($"Result: {result.ToString(CultureInfo.InvariantCulture)}");
		Console.WriteLine($"Written to: {Path.GetFullPath(outputPath)}");
	}
	catch (IOException ioException)
	{
		Console.WriteLine($"I/O error while writing result: {ioException.Message}");
	}
}
