# Tutorial 03 – Task 3 (HTTP Message Parser & Builder) – Exam Notes

## 1) What Task 3 asks

From `tutorial_03.md`, Task 3 requires implementing an HTTP message parser and builder:

- Parse incoming raw HTTP text into a model (`Parse`).
- Build a raw HTTP string from the model (`ToString`).
- Correctly differentiate between **request** and **response** messages.

Implemented with modern tooling in `tutorial_03/Task3-Solution`.

---

## 2) Modern project structure

- `Task3-Core` (`net10.0`): reusable HTTP message model + parse/build logic
- `Task3-Tests` (`xunit`): unit tests for template behavior and roundtrips
- `Task3-Cli` (`net10.0`): optional command-line tool for parse/build demos

Key files:

- `Task3-Core/HttpMessage.cs`
- `Task3-Tests/HttpMessageTests.cs`
- `Task3-Cli/Program.cs`

---

## 3) HTTP message model and differentiation

`HttpMessage` contains fields for both shapes:

- Request fields: `Method`, `Host`, `Resource`
- Response fields: `StatusCode`, `StatusMessage`
- Common fields: `Headers`, `Content`

Differentiation rule:

- If start line starts with `HTTP/` => **response**
- Otherwise => **request** (`METHOD RESOURCE HTTP/1.1`)

The helper `IsRequest` indicates current message type.

---

## 4) Parse logic (`HttpMessage(string message)`)

1. Normalize line endings (`\r\n` to `\n`).
2. Split header section and body at first blank line (`\n\n`).
3. Parse first line:
   - Response: `HTTP/1.1 <statusCode> <statusMessage>`
   - Request: `<method> <resource> HTTP/1.1`
4. Parse headers (`Name: Value`) into dictionary.
5. For requests, fill `Host` from the `Host` header.

Error handling:

- Throws `FormatException` for malformed start line or malformed header lines.

---

## 5) Build logic (`ToString()`)

`ToString()` reconstructs canonical HTTP text as:

1. Start line (request or response)
2. Header lines
3. Empty line separator
4. Content/body

Format output uses `\n` separators, matching the template tests.

---

## 6) Tests and what they prove

`HttpMessageTests.cs` includes:

1. `ParseResponseMessage_TemplateCase_Passes`
2. `ParseRequestMessage_TemplateCase_Passes`
3. `ResponseToString_RoundTrip_TemplateCase_Passes`
4. `RequestToString_RoundTrip_TemplateCase_Passes`
5. `BuildRequest_ThenParse_PreservesValues`

Coverage intent:

- Correct request/response differentiation
- Correct header/body parsing
- Stable parse -> stringify roundtrip
- Builder output parseability

Result: all tests pass.

---

## 7) How to run

From `tutorial_03/Task3-Solution`:

- Run tests:
  - `dotnet test .\Task3-Solution.slnx`

---

## 8) Optional CLI project (documented)

The optional CLI (`Task3-Cli`) was added and wired to `Task3-Core`.

### Commands

- `sample-request` -> prints a built example request
- `sample-response` -> prints a built example response
- `parse <raw_http_message>` -> parses and prints interpreted fields

### Examples

- `dotnet run --project .\Task3-Cli\Task3-Cli.csproj -- sample-request`
- `dotnet run --project .\Task3-Cli\Task3-Cli.csproj -- sample-response`
- `dotnet run --project .\Task3-Cli\Task3-Cli.csproj -- parse "POST /test HTTP/1.1\nHost: example.org\nContent-Length: 5\n\nhallo"`

Interactive mode is also available by running CLI without args.

---

## 9) Typical oral exam questions

### Q1: How do you distinguish request vs response when parsing?

By checking the start line. Response lines start with `HTTP/`, request lines start with a method token like `GET`/`POST`.

### Q2: Why is the blank line important?

It separates headers from body. Without this separator, parser cannot reliably detect where headers end.

### Q3: Why keep one class for both request and response?

Both share most structure (headers + content). A single model with clear type differentiation keeps parsing/building concise.

---

## 10) One-sentence exam summary

Task 3 is solved with a robust `HttpMessage` parser/builder that correctly handles request/response start lines, headers, body separation, roundtrip formatting, and includes an optional CLI for quick manual verification.
