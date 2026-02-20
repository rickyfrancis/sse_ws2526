## 🔽 Tutorial 02 🔒

**RPC and Middleware**

---

### Task 1

Get informed about the Proxy software design pattern (e.g. at [oodesign.com](http://www.oodesign.com/proxy-pattern.html)). Create a proxy for the Calculator class from the 1st tutorial. The proxy should cache the last 10 calculations and return the cached results when the new and the cached operation/operands pairs match. The **_Task1-Template.zip_** gives you a good starting point if you have not finished the 1st tutorial.

### Task 2

Answer the following questions briefly and accurately in your own words:

1. What is Middleware?
2. Which services are usually provided by Middleware?
3. What is Remote Procedure Call?

### Task 3

- What is Marshalling? Answer briefly and accurately in your own words.
- The **_Task3-Template.zip_** project implements a scenario, in which a client sends an object to a server, which formats the object and prints to console. The connection is established by middleware, which in addition takes care of typed objects' transfer. Read the code, implement the methods **_Marshall_** and **_Demarshall_**, and test your implementation.
- Beside the default printing functionality, the server provides two string operations: **`string concat(string arg1, string arg2, string arg3)`** and **`string substring(string str, string positionIndex)`**.
- Assume there are more of such operations, all only with string parameters. Extend your client towards RPC of these operations. Complete the placeholders marked with TODO. Define a message encoding scheme for RPC calls and use Reflection (cf. [codeproject.com](http://www.codeproject.com/Articles/17269/Reflection-in-C-Tutorial)) on the server side to invoke the methods.
