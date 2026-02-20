# Tutorial 03 - Task 4 Solution

Modernized runnable HTTP server for the cookie visit-counter scenario from Tutorial 03 Task 4.

## Project

- `Task4-Project` - runnable server (`GET /` + cookie-based visit counting)

## Prerequisites

- .NET SDK installed (project targets `net10.0`)

## Run server

From this folder:

```powershell
dotnet run --project .\Task4-Project\Task4-Project.csproj
```

Server listens on:

- `http://127.0.0.1:3000`

Stop with:

- `CTRL+C`

## Try the cookie scenario

1. Open `http://127.0.0.1:3000/` in a browser.
2. Refresh the page multiple times.
3. Visit count in the HTML response increases (`1st`, `2nd`, `3rd`, ...).

### Optional PowerShell check

```powershell
$response1 = Invoke-WebRequest -Uri http://127.0.0.1:3000/ -UseBasicParsing -SessionVariable s
$response2 = Invoke-WebRequest -Uri http://127.0.0.1:3000/ -UseBasicParsing -WebSession $s
$response1.Content
$response2.Content
```

## Implemented details

- `HttpServer.ReceiveRequest` handles:
  - `GET /` -> `200 OK` + `Set-Cookie: visit-count=<n>`
  - unknown path -> `404 Not Found`
- `HttpMessage` supports:
  - cookie read (`GetCookies`)
  - cookie write (`SetCookie`)
  - HTTP serialization with proper `\r\n` separators and `Content-Length`
- `TcpServer` uses UTF-8 without BOM for protocol-safe responses.
