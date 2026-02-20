# Tutorial 04 - Task 2.1 Solution

Modernized runnable SOAP 1.2 client for the `Add` operation (manual SOAP XML over HTTP, no code-generation tools).

## Project

- `Task2.1-Project` - console app with manual SOAP request creation and response parsing

## Prerequisites

- .NET SDK installed (project targets `net10.0`)
- Access to service endpoint (university network or VPN)

Service URL used by default:

- `http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/Service.asmx`

## Build

From this folder:

```powershell
dotnet build .\Task2.1-Solution.slnx
```

## Run

### Default run (`2 + 3`)

```powershell
dotnet run --project .\Task2.1-Project\Task2.1-Project.csproj
```

### Custom URL and operands

```powershell
dotnet run --project .\Task2.1-Project\Task2.1-Project.csproj -- "http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/Service.asmx" 10 20
```

Arguments:

1. `serviceUrl` (optional)
2. `a` (optional integer)
3. `b` (optional integer)

## What the app does

- Manually builds SOAP 1.2 envelope XML for `Add(intA, intB)`
- Sends HTTP POST with SOAP 1.2 `Content-Type`
- Parses `AddResult` from SOAP XML response
- Prints `Result: <value>` to console

## Notes

- No SOAP proxy/code-generation tooling is used.
- If service is unreachable from your network, the app prints a helpful VPN/network hint.
