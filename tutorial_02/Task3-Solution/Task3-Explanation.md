# Tutorial 02 – Task 3 (Marshalling + RPC + Reflection) – Exam Notes

## 0) Marshalling Theory (exam style)

### Exam-ready definition

**Marshalling** is the process of transforming in-memory objects into a transferable format (e.g., JSON) so they can be sent over a network, and reconstructing them on the receiver side (**demarshalling** / unmarshalling).

### Why marshalling is needed

- Network channels transfer bytes/strings, not language-native objects.
- Sender and receiver must agree on a message format.
- It preserves type and value information across process boundaries.

### In this task

- Objects (`double`, `string`, `string[]`, and RPC requests) are marshalled to JSON envelopes.
- Middleware demarshalls incoming JSON and forwards typed objects to server logic.

---

## 1) What Task 3 asks

From `tutorial_02.md`, Task 3 requires:

1. Explain marshalling.
2. Implement `Marshall` and `Demarshall` in middleware flow.
3. Extend client/server to support RPC operations (`concat`, `substring`) and make it generic enough for more string-based operations.
4. Use reflection on the server to invoke requested procedure names.

This is implemented in `tutorial_02/Task3-Solution`.

---

## 2) Solution structure (modern setup)

- `Task3-Project` (`net10.0`): runnable app with client, middleware, server, tcp transport.
- `Task3-Tests` (`xunit`): unit tests for codec and reflection-based RPC dispatch.

Key files:

- `Task3-Project/RpcCodec.cs`
- `Task3-Project/RpcRequest.cs`
- `Task3-Project/Middleware.cs`
- `Task3-Project/Server.cs`
- `Task3-Project/Client.cs`
- `Task3-Tests/RpcCodecTests.cs`
- `Task3-Tests/ServerRpcTests.cs`

---

## 3) Message encoding scheme

All messages are JSON envelopes:

```json
{
  "type": "double|string|stringArray|rpc",
  "payload": ...
}
```

Examples:

- Print double: `{"type":"double","payload":2.25}`
- Print string array: `{"type":"stringArray","payload":["a","b","c"]}`
- RPC call:
  `{"type":"rpc","payload":{"name":"concat","args":["a","b","c"]}}`

Why this is good:

- Single unified transport format
- Explicit type discriminator (`type`)
- Easy extensibility for new RPC operations

---

## 4) Core implementation walkthrough

### `RpcCodec` (`Marshall` / `Demarshall`)

`RpcCodec.Marshall(object)` maps supported CLR types to envelope JSON.
`RpcCodec.Demarshall(string)` parses envelope and rebuilds typed objects.

Supported payload object types:

- `double`
- `string`
- `string[]`
- `RpcRequest` (`name + args`)

### `Middleware.CallFunction(...)`

RPC client call path:

1. Build `RpcRequest`
2. Marshall to JSON
3. Send over TCP (`TcpRequest.Do`)
4. Return server response string

### `Server.HandleRequest(object request)`

- If request is `RpcRequest`: use reflection to find matching public method (`string` return type, string parameters).
- If not RPC: fallback to original print behavior.

Reflection dispatch supports case-insensitive method names and returns structured RPC errors for unknown procedures.

---

## 5) Reflection-based RPC behavior

Server RPC methods:

- `Concat(string arg1, string arg2, string arg3)`
- `Substring(string s, string startingPosition)`

Generic dispatch checks:

- method name matches requested procedure
- return type is `string`
- all parameters are `string`
- arity matches incoming argument count

This aligns with the task assumption: "there are more operations, all only with string parameters."

---

## 6) Tests and what they prove

### `RpcCodecTests`

- roundtrip for `double`
- roundtrip for `string`
- roundtrip for `string[]`
- roundtrip for `RpcRequest`

These prove marshalling/demarshalling correctness.

### `ServerRpcTests`

- reflection invocation for `concat`
- reflection invocation for `substring`
- unknown procedure handling (`RPC-ERROR`)
- non-RPC request still goes through print path

These prove RPC dispatch and backward compatibility with original printing behavior.

---

## 7) How to run

From `tutorial_02/Task3-Solution`:

- Run all tests:
  - `dotnet test .\Task3-Solution.slnx`

- Run demo app:
  - `dotnet run --project .\Task3-Project\Task3-Project.csproj`

Expected demo output includes:

- print responses for `double`, `string`, `string[]`
- RPC results:
  - `Concatenation Result: abc`
  - `Substring Result: world`

---

## 8) Typical oral exam questions

### Q1: What is marshalling in one sentence?

Converting runtime objects into a transferable representation and reconstructing them at receiver side.

### Q2: Why include `type` in the message?

Because payload alone is ambiguous; `type` tells the demarshaller which concrete object to reconstruct.

### Q3: Why reflection for RPC?

It enables procedure invocation by name at runtime without hard-coding each call path.

### Q4: How is this extensible?

Add new public string-returning methods with string parameters on server; client can call them using `CallFunction`.

---

## 9) One-sentence exam summary

Task 3 implements typed network transfer via JSON marshalling and a reflection-based RPC mechanism that can invoke string-parameter procedures dynamically while preserving the original middleware printing workflow.
