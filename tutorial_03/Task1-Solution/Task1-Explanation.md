# Tutorial 03 – Task 1 (URL Parsing & Generation) – Exam Notes

## 1) Theory answers (exam style)

### 1.1 What is the difference between URI and URL?

- A **URI** (Uniform Resource Identifier) is the general concept for identifying a resource.
- A **URL** (Uniform Resource Locator) is a specific kind of URI that tells **where** the resource is and usually **how** to access it (protocol + location).
- In short: every URL is a URI, but not every URI is necessarily a URL.

### 1.2 What is the meaning of the URL scheme?

- The **scheme** is the prefix before `://` (e.g., `http`, `https`, `ftp`).
- It defines the protocol/communication rules used to access the resource.
- It can also imply defaults (for example, HTTP commonly uses port 80, HTTPS port 443).

### 1.3 What, why, and how should be encoded in URLs?

- **What**: characters not allowed in URL components (spaces, non-ASCII, reserved/special characters in wrong context).
- **Why**: to keep URLs syntactically valid, unambiguous, and safely transferable across clients/servers.
- **How**: percent-encoding (`%HH`), where bytes are represented in hexadecimal (typically UTF-8 bytes).
  - Example: space -> `%20`, `ä` -> `%C3%A4`

---

## 2) What Task 1 asks

From `tutorial_03.md`, Task 1 requires:

1. Parse URL into components using a **regular expression**.
2. Build URL string back in `ToString()`.
3. Implement `Encode(string)` for characters outside allowed set.
4. Implement `Decode(string)` for `%` escaped bytes.

Implemented in `tutorial_03/Task1-Solution/Task1-Project`.

---

## 3) Project and tooling (modern)

Solution:

- `tutorial_03/Task1-Solution/Task1-Solution.slnx`

Project:

- `tutorial_03/Task1-Solution/Task1-Project/Task1-Project.csproj`
- Target framework: `net10.0`
- Test stack: `xunit 2.9.3`, `Microsoft.NET.Test.Sdk 18.0.1`, `xunit.runner.visualstudio 3.1.5`

Main files:

- `Url.cs`
- `UrlTests.cs`

---

## 4) Implementation walkthrough

### 4.1 Parsing constructor (`Url(string urlStr)`)

- Uses regex to split into named groups:
  - `scheme`, `host`, optional `port`, optional `path`, optional `query`, optional `fragment`
- If regex does not match: throws `FormatException`.
- Parsed path/query/fragment are decoded using `Decode(...)`.

Regex used:

```regex
^(?<scheme>[a-zA-Z][a-zA-Z0-9+\-.]*)://(?<host>[^/:?#]+)(?::(?<port>\d+))?(?<path>/[^?#]*)?(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?$
```

### 4.2 URL generation (`ToString()`)

- Rebuilds URL in order: scheme + host + optional port + path + optional query + optional fragment.
- Omits `:80` (default HTTP port) for cleaner canonical output.
- Encodes components while preserving structural separators:
  - path preserves `/`
  - query preserves `&` and `=`

### 4.3 Encoding (`Encode(string s)`)

- Allowed characters are exactly the task’s `VALID_CHARACTERS` set.
- Any other char is UTF-8 encoded and converted to `%HH` per byte.

### 4.4 Decoding (`Decode(string s)`)

- Reads `%HH` sequences.
- Reconstructs byte stream and decodes UTF-8 back to characters.
- Leaves non-escaped characters as-is.

---

## 5) Tests and coverage

`UrlTests.cs` includes:

- `ParseAndToString_TemplateExample_Passes`
  - uses provided sample and validates all fields + `ToString()` roundtrip.
- `Encode_EncodesCharactersOutsideValidSet`
  - checks space, slash, and umlaut encoding.
- `Decode_DecodesEscapedCharacters`
  - validates reverse conversion.
- `Parse_WithoutOptionalParts_UsesDefaults`
  - checks optional parts and default port behavior.

Test result: all tests pass.

---

## 6) How to run

From `tutorial_03/Task1-Solution`:

- `dotnet test .\Task1-Solution.slnx`

### Run URL tool as a program (CLI)

Task 1 now also includes `Task1-Cli` for manual parse/encode/decode operations.

- Parse a URL into components:
  - `dotnet run --project .\Task1-Cli\Task1-Cli.csproj -- parse "http://www.tu-chemnitz.de:8080/ein%20test?my-name=my-value&a=1#id"`

- Encode text:
  - `dotnet run --project .\Task1-Cli\Task1-Cli.csproj -- encode "ein test/ä"`

- Decode text:
  - `dotnet run --project .\Task1-Cli\Task1-Cli.csproj -- decode "ein%20test%2F%C3%A4"`

- Interactive mode:
  - `dotnet run --project .\Task1-Cli\Task1-Cli.csproj`
  - then use commands:
    - `parse <url>`
    - `encode <text>`
    - `decode <text>`
    - `exit`

---

## 7) One-sentence exam summary

Task 1 is solved with a regex-based URL parser plus robust percent encode/decode logic, and the generated URL string preserves component separators while correctly escaping data characters.
