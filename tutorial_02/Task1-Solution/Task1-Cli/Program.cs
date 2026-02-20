using System.Globalization;
using Task1Solution;

ICalculator calculator = new Proxy();

if (args.Length == 3)
{
	RunSingleOperation(args[0], args[1], args[2]);
	return;
}

Console.WriteLine("Task1 Proxy Calculator CLI");
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
}
