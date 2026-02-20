# Tutorial 03 - Task 1 Solution

Modernized solution for URL parsing, generation, encoding, and decoding, based on the template requirements.

## Projects

- `Task1-Project` - URL model + parse/`ToString`/encode/decode logic
- `Task1-Cli` - optional CLI for manual parse/encode/decode demos

## Prerequisites

- .NET SDK installed (projects target `net10.0`)

## Run tests

From this folder:

```powershell
dotnet test .\Task1-Solution.slnx
```

## Run CLI

### Parse a URL

```powershell
dotnet run --project .\Task1-Cli\Task1-Cli.csproj -- parse "http://www.tu-chemnitz.de:8080/ein%20test?my-name=my-value&a=1#id"
```

### Encode text

```powershell
dotnet run --project .\Task1-Cli\Task1-Cli.csproj -- encode "ein test/ä"
```

### Decode text

```powershell
dotnet run --project .\Task1-Cli\Task1-Cli.csproj -- decode "ein%20test%2F%C3%A4"
```

### Interactive mode

```powershell
dotnet run --project .\Task1-Cli\Task1-Cli.csproj
```

Commands in interactive mode:

- `parse <url>`
- `encode <text>`
- `decode <text>`
- `exit`

## Notes

- URL parser uses a regular expression to split scheme, host, port, path, query, and fragment.
- `Encode` uses UTF-8 percent-encoding for characters outside the allowed set.
- `ToString()` preserves URL separators (e.g., `/`, `&`, `=`) while encoding data characters.
