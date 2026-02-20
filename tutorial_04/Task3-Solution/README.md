# Tutorial 04 - Task 3 Solution

Distributed SOAP messaging solution for robust/simple one-way MEP with selective encryption.

## Projects

- `HttpLib` - shared transport, HTTP helpers, and encryption helpers
- `Broker` - forwards updates and returns SOAP receipt notifications (`BUSY` / `RECIEVED`)
- `Server` - sends stock updates and retransmits on BUSY
- `Client1` - processes plaintext + encrypted updates (decrypts)
- `Client2` - processes plaintext updates, ignores encrypted ones

## Technology

- .NET 10 (`net10.0`)
- Multi-project solution: `Task3-Solution.slnx`

## Build

From `tutorial_04/Task3-Solution`:

```powershell
dotnet build Task3-Solution.slnx
```

## Run scenario

Open **4 terminals** in `tutorial_04/Task3-Solution` and run:

Terminal 1:

```powershell
dotnet run --project .\Client1\Client1.csproj
```

Terminal 2:

```powershell
dotnet run --project .\Client2\Client2.csproj
```

Terminal 3:

```powershell
dotnet run --project .\Broker\Broker.csproj
```

Terminal 4:

```powershell
dotnet run --project .\Server\Server.csproj
```

## Expected behavior

- Server may print `broker is busy. repeating transmission...` when broker responds with BUSY.
- Broker sends SOAP acknowledgements and forwards updates to both clients.
- Client1 prints normal updates and decrypted encrypted updates.
- Client2 prints normal updates and logs `encrypted update ignored.` for encrypted messages.

## Task requirement mapping

1. Broker `ReceiveMessage` sends SOAP-based receipt notifications: implemented.
2. Server + Client1 encrypt/decrypt with shared secret: implemented.
3. Client2 ignores encrypted messages, based on SOAP header signaling from server: implemented.
