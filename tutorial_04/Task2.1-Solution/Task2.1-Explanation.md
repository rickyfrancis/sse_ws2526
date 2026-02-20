# Tutorial 04 – Task 2.1 (Manual SOAP 1.2 Client) – Exam Notes

## 1) What Task 2.1 asks

From `tutorial_04.md`, Task 2.1 requires:

1. Call `Add(intA, intB)` on the SOAP web service.
2. Send **manually constructed SOAP 1.2 XML** over HTTP.
3. Do not use code-generation tools or third-party SOAP client libraries.
4. Print result to console.

Implemented in `tutorial_04/Task2.1-Solution/Task2.1-Project`.

---

## 2) Architecture used in solution

- `Program.cs`: runnable entry point
- `AddServiceClient.cs`: builds SOAP envelope and parses SOAP response
- `HttpRequest.cs`: manual HTTP request over raw TCP
- `HttpMessage.cs`: HTTP request/response serializer/parser
- `TcpRequest.cs`: socket-level send/receive
- `Url.cs`: URL parsing helper

No SOAP code generation tools are used.

---

## 3) SOAP 1.2 request construction

`AddServiceClient.Add(int a, int b)` builds XML envelope manually:

- Envelope namespace: `http://www.w3.org/2003/05/soap-envelope`
- Operation namespace: `http://tempuri.org/`
- Body payload:
  - `<Add>`
  - `<intA>...</intA>`
  - `<intB>...</intB>`

HTTP headers include:

- `Content-Type: application/soap+xml; charset=utf-8; action="http://tempuri.org/Add"`
- `Accept: application/soap+xml, text/xml`

This is compliant with SOAP 1.2 over HTTP style.

---

## 4) Response parsing

After receiving HTTP response:

1. Parse response body XML (`XDocument`).
2. Extract `AddResult`:
   - preferred XPath with service namespace
   - fallback via `local-name()='AddResult'` for namespace tolerance
3. Convert to integer and return.

If parsing fails, a clear exception is thrown.

---

## 5) Runnable behavior

`Program` is executable and supports optional args:

- `arg0`: service URL
- `arg1`: integer `a`
- `arg2`: integer `b`

Defaults:

- URL: `http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/Service.asmx`
- `a=2`, `b=3`

On success:

- prints `Result: <sum>`

On failure (e.g., outside VPN/university network):

- prints a readable error message and VPN hint.

---

## 6) How to run

From `tutorial_04/Task2.1-Solution`:

- Build:
  - `dotnet build .\Task2.1-Solution.slnx`

- Run with defaults:
  - `dotnet run --project .\Task2.1-Project\Task2.1-Project.csproj`

- Run with custom values:
  - `dotnet run --project .\Task2.1-Project\Task2.1-Project.csproj -- "http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/Service.asmx" 10 20`

---

## 7) Typical oral exam questions

### Q1: Why is this "manual SOAP"?

Because the SOAP envelope XML and HTTP headers are hand-written in code; no WSDL-generated proxy is used.

### Q2: Why include `action` in SOAP 1.2 content-type?

SOAP 1.2 carries action semantics as a parameter of `application/soap+xml`.

### Q3: Where is SOAP in the stack?

SOAP is an application-layer message format carried over transport protocols such as HTTP.

---

## 8) One-sentence exam summary

Task 2.1 is solved by manually constructing a SOAP 1.2 `Add` request, sending it over raw HTTP/TCP, parsing `AddResult` from the XML response, and printing the sum in a runnable console app.
