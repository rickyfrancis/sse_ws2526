# Tutorial 03 - Task 3 Solution

Modernized solution for HTTP message parsing and building (request/response differentiation), based on the template requirements.

## Projects

- `Task3-Core` - HTTP message model + `Parse`/`ToString` logic
- `Task3-Tests` - xUnit tests for parser/builder behavior
- `Task3-Cli` - optional CLI for manual parsing/building demos

## Prerequisites

- .NET SDK installed (project targets `net10.0`)

## Run tests

From this folder:

```powershell
dotnet test .\Task3-Solution.slnx
```

## Run CLI

### Sample request output

```powershell
dotnet run --project .\Task3-Cli\Task3-Cli.csproj -- sample-request
```

### Sample response output

```powershell
dotnet run --project .\Task3-Cli\Task3-Cli.csproj -- sample-response
```

### Parse a raw HTTP message

Use `\n` in the argument for line breaks:

```powershell
dotnet run --project .\Task3-Cli\Task3-Cli.csproj -- parse "POST /test HTTP/1.1\nHost: example.org\nContent-Length: 5\n\nhallo"
```

### Interactive mode

```powershell
dotnet run --project .\Task3-Cli\Task3-Cli.csproj
```

Commands in interactive mode:

- `parse <raw_http_message>`
- `sample-request`
- `sample-response`
- `exit`

## Notes

- Parser accepts both `\n` and `\r\n` internally.
- Message builder emits `\n` line endings to match template-style tests.
