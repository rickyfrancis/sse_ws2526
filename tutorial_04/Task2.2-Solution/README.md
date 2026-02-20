# Tutorial 04 - Task 2.2 Solution

Generic SOAP 1.2 client implementation for Tutorial 04 Task 2.2.

## Structure

- `Task2.2-Solution.slnx` — solution file
- `Task2.2-Project` — runnable console app containing:
  - `SoapClient.cs` (generic `Invoke` implementation)
  - `HttpRequest.cs`, `HttpMessage.cs`, `TcpRequest.cs`, `Url.cs` (manual HTTP/TCP transport)
  - `Program.cs` (calls two SOAP services)

## Prerequisites

- .NET SDK 10 or newer
- Access to university network or VPN (for service endpoints)

## Run

From this folder:

```powershell
dotnet build Task2.2-Solution.slnx
dotnet run --project .\Task2.2-Project\Task2.2-Project.csproj
```

Expected output:

- `Concatenator Result: bla:5:?`
- `Add Result: 25`

## Task mapping

Implements `SoapClient.Invoke` to call SOAP services using:

- URL
- service namespace
- operation name
- named parameters (`string` and `int`)

The method builds SOAP 1.2 request envelopes manually, sends them over HTTP, parses SOAP responses, detects SOAP Faults, and returns the operation result as `int` or `string`.
