# Tutorial 04 — Task 2.2 Explanation

## Goal

Implement a **generic SOAP 1.2 client** by completing `SoapClient.Invoke` so it can call different SOAP services using:

- service URL
- service namespace
- operation name
- named parameter list

Only `string` and `int` parameter/result values are required.

## What was implemented

A modern runnable solution was created in:

- `tutorial_04/Task2.2-Solution/Task2.2-Solution.slnx`
- `tutorial_04/Task2.2-Solution/Task2.2-Project`

Key implementation is in `SoapClient.cs`:

1. Build SOAP 1.2 envelope dynamically from operation and parameter dictionary.
2. Send message via manual HTTP over TCP (`HttpRequest` + `TcpRequest`).
3. Parse SOAP response body and return typed value (`int` when possible, otherwise `string`).
4. Detect SOAP Fault responses and throw meaningful errors.

## How `SoapClient.Invoke` works

### 1) Build SOAP Action and Envelope

- SOAP Action is generated from namespace + operation name.
- Request body is generated as:
  - `<Operation xmlns="serviceNamespace">`
  - child elements for each named parameter
- Parameter values are restricted to `int` and `string` (as required).
- XML values are escaped for safety.

### 2) Send request manually over HTTP

`HttpRequest.Post`:

- resolves host to IPv4
- creates a raw HTTP request with headers:
  - `Content-Type: application/soap+xml; charset=utf-8; action="..."`
  - `Accept: application/soap+xml, text/xml`
- sends request over raw TCP socket

`HttpMessage` handles:

- serialization to HTTP text with proper CRLF line endings
- parsing response status, headers, and content

### 3) Parse response generically

- Parse XML via `XDocument`.
- Check for `<Fault>` inside SOAP body and throw if present.
- Find `OperationResponse` element (or fallback to first body element).
- Find `...Result` element (or fallback to first child element).
- Convert to `int` if possible, otherwise return as `string`.

## Runnable demonstration in Program

`Program.cs` demonstrates both required test calls:

1. **ConcatenatorService** `Concatenate(joinSymbol, first, second, third)`
2. **SoapWebService** `Add(a, b)`

On success, console output is:

- `Concatenator Result: bla:5:?`
- `Add Result: 25`

## Why this satisfies the task

- Uses **manually constructed SOAP messages** over HTTP.
- Uses **no code generation** and no third-party SOAP frameworks.
- Works for **any SOAP service** that follows the provided invocation pattern.
- Supports requested primitive parameter/result types (`string`, `int`).
- Includes a **runnable application** (not only tests).
