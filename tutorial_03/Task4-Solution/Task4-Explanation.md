# Tutorial 03 – Task 4 (HTTPS, Cookies, Stateful HTTP Scenario) – Exam Notes

## 1) What are the goals of HTTPS and how are they achieved?

### Goals

- **Confidentiality**: protect data from eavesdropping.
- **Integrity**: detect/avoid tampering in transit.
- **Authentication**: verify server identity (optionally client identity).

### How achieved

- HTTPS = HTTP over **TLS**.
- TLS handshake negotiates cryptographic parameters and session keys.
- Server proves identity via X.509 certificate signed by trusted CA.
- Symmetric encryption protects payload after handshake.
- MAC/AEAD protects integrity and authenticity of transmitted records.

---

## 2) Difference between HTTP and HTTPS request/response messages

- At HTTP semantics level (methods, headers, status codes, body), messages are conceptually the same.
- Difference is transport layer:
  - HTTP: plaintext over TCP.
  - HTTPS: HTTP bytes are encapsulated in encrypted TLS records.
- In HTTPS, intermediates without TLS termination cannot read/modify headers or body content.

---

## 3) Purpose of HTTP cookies

- Cookies store small key-value state on client side for a domain/path.
- They allow stateless HTTP requests to carry state across calls.
- Typical uses:
  - session tracking/authentication
  - personalization
  - visit counters / lightweight per-user state

---

## 4) Cookie scenario (missing headers/status codes)

Scenario: each repeated request should return how many times client visited before/currently.

### Typical flow

1. **First request**
   - Client -> Server: `GET / HTTP/1.1`
   - (No `Cookie` header yet)

2. **First response**
   - Status: `HTTP/1.1 200 OK`
   - Header: `Set-Cookie: visit-count=1`
   - Body: "This is your 1st visit ..."

3. **Second request**
   - Client -> Server: `GET / HTTP/1.1`
   - Header: `Cookie: visit-count=1`

4. **Second response**
   - Status: `HTTP/1.1 200 OK`
   - Header: `Set-Cookie: visit-count=2`
   - Body: "This is your 2nd visit ..."

5. **Unknown route**
   - Status: `HTTP/1.1 404 Not Found`

This is exactly what the solution implements.

---

## 5) Implementation summary (Task4-Solution)

Implemented in `tutorial_03/Task4-Solution/Task4-Project`.

### Core behavior

- Parse incoming HTTP request in `HttpServer.HandleRequest`.
- For `GET /`:
  - read incoming cookie map (`HttpMessage.GetCookies`)
  - increment `visit-count`
  - return `200 OK` HTML response
  - set cookie in response via `HttpMessage.SetCookie`
- For all other routes:
  - return `404 Not Found`

### HTTP message updates

- Implemented `SetCookie(name, value)`
- Implemented `GetCookies()` parsing `Cookie` / `Set-Cookie`
- `ToString()` now emits valid HTTP with `\r\n` separators and `Content-Length`

### Transport correctness fix

- TCP stream uses UTF-8 **without BOM** to avoid protocol-violation errors in strict clients.

---

## 6) How to run

From `tutorial_03/Task4-Solution`:

- Start server:
  - `dotnet run --project .\Task4-Project\Task4-Project.csproj`

Then request `http://127.0.0.1:3000/` repeatedly (browser or client) and observe incrementing visit count.

---

## 7) One-sentence exam summary

Task 4 extends a basic HTTP server with cookie-based state so repeated `GET /` requests return incrementing visit counts, while preserving HTTP semantics and using the correct `Cookie` / `Set-Cookie` headers and status codes (`200`, `404`).
