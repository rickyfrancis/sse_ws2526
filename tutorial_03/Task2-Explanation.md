# Tutorial 03 — HTTP Methods & Headers — Study Guide

---

## Background: What Is HTTP?

**HTTP (HyperText Transfer Protocol)** is the foundation of data communication on the web. It is a **request-response protocol**: a client (typically a browser or application) sends a request to a server, and the server sends back a response. Every request consists of a **method** (what action to perform), a **URL** (what resource to act on), **headers** (metadata about the request), and optionally a **body** (data to send).

HTTP is **stateless** — each request is independent and the server retains no memory of previous requests. Any state management (sessions, authentication) must be handled explicitly on top of the protocol.

Before looking at individual methods and headers, three key properties need to be understood clearly, because the exam question asks about them directly.

---

## Three Key Properties: Safe, Idempotent, Cacheable

### Safe

A method is **safe** if it does not modify the state of the server. A safe request is purely **read-only** from the server's perspective. The client is guaranteed that calling a safe method will have no side effects — no data is created, changed, or deleted. Because of this, safe methods can be called freely by browsers, crawlers, and prefetch mechanisms without concern.

_Importantly: safe does not mean the server does nothing at all — logging a request or incrementing an analytics counter is acceptable — it just means no meaningful application state changes._

### Idempotent

A method is **idempotent** if calling it **multiple times produces the same result** as calling it once. In other words, repeating the request has no additional effect after the first call. This is a crucial property for reliability: if a network request times out and you don't know whether it reached the server, you can safely retry an idempotent request without fear of causing duplicate side effects.

_Note: all safe methods are also idempotent by definition, but not all idempotent methods are safe — DELETE is idempotent but not safe._

### Cacheable

A method is **cacheable** if its responses can be stored by the client or an intermediary (like a proxy) and reused for equivalent future requests, avoiding a round-trip to the server. Caching is only sensible for methods that retrieve data and where the response is unlikely to change between requests.

---

## HTTP Methods

### GET

**Semantics:** GET requests the **retrieval of a resource** at the specified URL. It asks the server to send back a representation of that resource — an HTML page, a JSON object, an image, etc. The request carries no body; all parameters are encoded in the URL itself (as query strings).

```
GET /products?category=books HTTP/1.1
Host: shop.example.com
```

- **Safe:** ✅ Yes — GET only reads, never modifies.
- **Idempotent:** ✅ Yes — calling it ten times returns the same resource (assuming it hasn't changed on the server).
- **Cacheable:** ✅ Yes — this is the primary method for which caching was designed. Browsers aggressively cache GET responses.

---

### HEAD

**Semantics:** HEAD is identical to GET in every way, except the server returns **only the headers — no body**. It's used to retrieve metadata about a resource without downloading its content. Common use cases include checking whether a resource exists (via status code), checking when it was last modified, or finding out its size (`Content-Length`) before deciding to download it.

```
HEAD /files/large-video.mp4 HTTP/1.1
Host: media.example.com
```

- **Safe:** ✅ Yes — read-only, just like GET.
- **Idempotent:** ✅ Yes — same reasoning as GET.
- **Cacheable:** ✅ Yes — the headers returned can be cached.

---

### POST

**Semantics:** POST **submits data to the server** to be processed, typically causing a **state change or side effect**. It is the general-purpose method for creating a new resource, submitting a form, uploading a file, or triggering an action. The data is sent in the request body. The server decides what to do with it — commonly creating a new resource and returning its URL in a `Location` header.

```
POST /orders HTTP/1.1
Host: shop.example.com
Content-Type: application/json

{ "item": "book_42", "quantity": 2 }
```

- **Safe:** ❌ No — POST causes side effects by design (creating/modifying data).
- **Idempotent:** ❌ No — submitting the same POST request twice typically creates two separate resources or triggers the action twice (two orders, two emails sent).
- **Cacheable:** ⚠️ Rarely — POST responses are generally not cached by default, though the HTTP spec technically permits it under specific conditions. In practice, treat POST as non-cacheable.

---

### PUT

**Semantics:** PUT **creates or completely replaces** the resource at the specified URL with the data in the request body. Unlike POST, the client specifies the exact URL of the resource. If the resource already exists, it is fully overwritten with the new representation. If it doesn't exist, a new resource is created at that URL.

```
PUT /users/42 HTTP/1.1
Host: api.example.com
Content-Type: application/json

{ "name": "Ricky", "email": "ricky@example.com" }
```

- **Safe:** ❌ No — PUT modifies or creates server state.
- **Idempotent:** ✅ Yes — sending the same PUT request five times results in the same final state as sending it once. The fifth call doesn't create a fifth copy; it just re-sets the resource to the same value.
- **Cacheable:** ❌ No — PUT modifies state, so cached responses would be stale.

---

### DELETE

**Semantics:** DELETE **removes the resource** at the specified URL from the server. After a successful DELETE, subsequent GET requests to that URL should return a 404 (Not Found).

```
DELETE /users/42 HTTP/1.1
Host: api.example.com
```

- **Safe:** ❌ No — it permanently modifies server state by removing a resource.
- **Idempotent:** ✅ Yes — deleting the same resource twice has the same outcome as deleting it once: the resource is gone. The second call might return a 404, but the server state is the same.
- **Cacheable:** ❌ No.

---

## Quick Reference Table

| Method     | Safe | Idempotent | Cacheable | Primary Purpose                          |
| ---------- | :--: | :--------: | :-------: | ---------------------------------------- |
| **GET**    |  ✅  |     ✅     |    ✅     | Retrieve a resource                      |
| **HEAD**   |  ✅  |     ✅     |    ✅     | Retrieve headers only, no body           |
| **POST**   |  ❌  |     ❌     |   ❌\*    | Submit data / create a resource          |
| **PUT**    |  ❌  |     ✅     |    ❌     | Replace/create a resource at a known URL |
| **DELETE** |  ❌  |     ✅     |    ❌     | Remove a resource                        |

_POST is technically cacheable under specific conditions but almost never is in practice._

---

## HTTP Headers

HTTP headers are **key-value pairs** sent at the beginning of both requests and responses, providing metadata about the message. They tell the recipient things like: what format the body is in, how large it is, who is sending it, where to redirect, and much more. Understanding headers is essential to understanding how HTTP actually works in practice.

---

### `Host`

**Direction:** Request header

**Purpose:** Specifies the **domain name (and optionally the port)** of the server the client wants to reach.

```
Host: www.example.com:8080
```

This header is **mandatory in HTTP/1.1** and exists because multiple websites can be hosted on the same physical server and IP address (virtual hosting). Without the `Host` header, the server would have no way to know which website the request is intended for. When your browser connects to an IP address and sends a request, the `Host` header tells the server: "I want the site at _this_ domain name, not just whatever's at this IP."

---

### `Content-Type`

**Direction:** Request and Response header

**Purpose:** Declares the **media type (MIME type) of the body** being sent, telling the recipient how to interpret or parse the data.

```
Content-Type: application/json
Content-Type: text/html; charset=UTF-8
Content-Type: multipart/form-data; boundary=----FormBoundary
```

Without this header, the recipient would have to guess the format of the body — which is unreliable and dangerous. Common values include `application/json` (JSON data), `text/html` (HTML document), `application/x-www-form-urlencoded` (form submission), and `image/png` (PNG image). The optional `charset` parameter specifies the text encoding (e.g., UTF-8).

---

### `Content-Length`

**Direction:** Request and Response header

**Purpose:** Specifies the **exact size of the request or response body in bytes**.

```
Content-Length: 348
```

This tells the recipient how many bytes to read from the stream before the body ends. It is important for reliable data transfer — without it, the recipient doesn't know when the body is complete and might either read too little (truncating data) or wait forever for more. For responses, it also allows the browser to display a progress bar (since it knows the total size). When using chunked transfer encoding (where data is sent in pieces), `Content-Length` is omitted since the total size isn't known upfront.

---

### `Accept`

**Direction:** Request header

**Purpose:** Tells the server **what media types the client is willing to receive** in the response, in order of preference.

```
Accept: text/html, application/xhtml+xml, application/json;q=0.9, */*;q=0.8
```

This is part of HTTP's **content negotiation** mechanism. The server reads the `Accept` header and responds with the best matching format it can provide. The optional `q` (quality) values indicate preference weights from 0 to 1 — higher means more preferred. `*/*` means "any format is acceptable." If the server cannot satisfy any of the requested types, it responds with `406 Not Acceptable`. This is how the same endpoint can return HTML to a browser and JSON to an API client based purely on the `Accept` header.

---

### `User-Agent`

**Direction:** Request header

**Purpose:** Identifies the **client software** making the request — typically the browser name, version, and operating system.

```
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/120.0.0.0
```

Servers use this information to tailor responses — for example, serving different CSS to different browsers, blocking known bots, serving a mobile layout to a mobile device, or logging analytics about which browsers your users use. It can also be used (and abused) for fingerprinting. API clients often set a custom `User-Agent` string to identify their application (e.g., `MyApp/2.1.0`). This header is informational and technically optional — clients can send anything they want (or nothing), so servers should not rely on it for security decisions.

---

### `Location`

**Direction:** Response header

**Purpose:** Indicates the **URL the client should go to next**, used in two main scenarios: redirections and resource creation.

```
Location: https://www.example.com/new-page
Location: /orders/order-99
```

In a **redirect** (HTTP status codes 301 Moved Permanently, 302 Found, 307 Temporary Redirect, etc.), `Location` tells the client the new URL of the resource — the browser automatically follows it. In a **resource creation** response (HTTP 201 Created, typically in response to a POST), `Location` points to the URL of the newly created resource, letting the client know where to find it. Without `Location`, a redirect would have nowhere to redirect to, making it effectively useless.

---

## Headers Quick Reference

| Header           | Direction | Purpose                                                                      |
| ---------------- | --------- | ---------------------------------------------------------------------------- |
| `Host`           | Request   | Identifies the target website by domain name (essential for virtual hosting) |
| `Content-Type`   | Both      | Declares the MIME type/format of the body being sent                         |
| `Content-Length` | Both      | States the exact byte size of the body                                       |
| `Accept`         | Request   | Tells the server which response formats the client can handle                |
| `User-Agent`     | Request   | Identifies the client application (browser, app, bot)                        |
| `Location`       | Response  | Provides a URL for redirects or newly created resources                      |

---

## Key Takeaways for Your Exam

- **Safe** = read-only, no server state change. Only GET and HEAD are safe.
- **Idempotent** = repeating the request has no additional effect. GET, HEAD, PUT, and DELETE are idempotent. POST is not.
- **Cacheable** = response can be stored and reused. GET and HEAD are cacheable; POST, PUT, DELETE are not.
- **POST vs PUT**: POST creates a resource at a server-decided URL; PUT places/replaces a resource at a client-specified URL. POST is not idempotent; PUT is.
- **DELETE is idempotent but not safe** — deleting twice leaves the same state as deleting once, but it does cause a side effect.
- **Host** is mandatory in HTTP/1.1 and enables virtual hosting. **Content-Type** says what format the body is. **Accept** says what formats the client wants back. **Location** is used for redirects and POST-created resource URLs.
