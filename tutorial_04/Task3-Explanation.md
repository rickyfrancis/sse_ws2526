# Tutorial 04 — Task 3 Explanation

## Goal

Implement robust and simple one-way MEP behavior in the distributed SOAP system:

1. Broker sends SOAP receipt notifications (`BUSY` or `RECIEVED`) back to server.
2. Some server messages are encrypted so broker cannot read business content.
3. Client 1 can decrypt encrypted messages.
4. Client 2 cannot decrypt and must ignore encrypted messages.

## Implemented solution

A modernized runnable solution is provided in `tutorial_04/Task3-Solution` (targeting .NET 10).

### 1) Broker SOAP receipt notifications

File: `Task3-Solution/Broker/Broker.cs`

`ReceiveMessage` now returns SOAP acknowledgments:

- If broker is overloaded (simulated by random): returns SOAP body with `<State>BUSY</State>`.
- Otherwise forwards message to both clients and returns SOAP body with `<State>RECIEVED</State>`.

This enables robust one-way behavior from server to broker: server can retry until it receives success.

### 2) Server retry + encryption signaling

File: `Task3-Solution/Server/Server.cs`

#### Retry logic

- Server sends stock updates to broker.
- It parses broker SOAP response in `CheckIfClientAcknowledgedReciept`.
- If state is `BUSY`, it retransmits (up to 15 tries).
- If state is `RECIEVED`/`RECEIVED`, it stops retransmitting.

#### Header signaling for encryption

When `encrypted == true`, server adds SOAP header:

- `<sec:RequiresEncryption xmlns:sec="http://example.org/security">true</sec:RequiresEncryption>`

This header tells receivers that body needs decryption before processing.

#### Body encryption

- Normal messages: body contains `<ex:StockUpdate>...</ex:StockUpdate>`.
- Encrypted messages: server encrypts the stock update XML using `Encryption.Encrypt` and wraps it in:
  - `<sec:EncryptedPayload xmlns:sec="http://example.org/security">...</sec:EncryptedPayload>`

So broker only forwards opaque encrypted payload and cannot interpret business data.

### 3) Client 1 decrypts when needed

File: `Task3-Solution/Client1/Client1.cs`

- Detects encryption requirement by checking header element `RequiresEncryption == true`.
- If encrypted:
  - reads `EncryptedPayload`
  - decrypts using shared key via `Encryption.Decrypt`
  - parses decrypted XML and extracts company and price
- If not encrypted:
  - parses normal stock update directly

Client 1 therefore processes both plaintext and encrypted updates.

### 4) Client 2 ignores encrypted updates

File: `Task3-Solution/Client2/Client2.cs`

- Checks same `RequiresEncryption` header.
- If encrypted: prints `encrypted update ignored` and returns.
- If plaintext: parses and prints stock update as normal.

This matches the requirement that client 2 has no decryption capabilities.

## Runtime verification performed

Distributed run was executed with all four apps:

- `Client1` (port 15001)
- `Client2` (port 15002)
- `Broker` (port 14000)
- `Server` (port 13000 + sender)

Observed behavior:

- Server printed retry messages when broker answered `BUSY`.
- Broker received and forwarded plaintext and encrypted SOAP messages.
- Client 1 output included decrypted encrypted updates (e.g., `TW,30.0`, `LI,15.0`).
- Client 2 output showed plaintext update and ignored encrypted updates.

This demonstrates all three task requirements are fulfilled.
