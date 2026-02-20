## 🔽 Tutorial 06 🔒

**WS-\***

---

### Task 1

Complete the implementation of MetadataReader.cs in project **Task1-Template.zip**, which should be able to read metadata using WS-MetadataExchange from a given endpoint (e.g. [http://pauline.informatik.tu-chemnitz.de/WcfAddService/Service1.svc/mex](http://pauline.informatik.tu-chemnitz.de/WcfAddService/Service1.svc/mex)). Print content of all _`MetadataSection`_-elements to the console. For help you can consider _`MetadataResponse.xml`_ within **Task1-Template.zip**.

> ℹ️ **Info**
> The service link is accessible only from the university network.

### Task 2

1. Record the traffic between a client from the project **WcfAddClient.zip** and the Web service from "**Task 1**". The communication requires encryption using the server's public key (from _`wcfaddservice.p12`_). Import the provided server certificate before starting the project _(Double click on the file → Next → Next → Password: 1111 → Automatically select... → Next → Finish)._
2. Use e.g. [Wireshark](http://www.wireshark.org/) or [Fiddler](http://www.fiddler2.com/fiddler2/) to record the traffic between the client and the service. Which standards (besides SOAP) have you recognized in the request/response messages? What are they used for?

> ⚠️ **Danger**
> Note: **WcfAddClient** requires tools that are not yet available for .NET Core, i.e., it only works on Windows. The executable **WcfAddClient.exe** is also provided for convenience.

### Task 3

Inform yourself about [XML Encryption and Signature](http://users.dcc.uchile.cl/~pcamacho/tutorial/web/xmlsec/xmlsec.html). The bundle **XMLSec.zip** contains a command [tool](http://www.aleksey.com/xmlsec/) for signing and encrypting XML files. Using the given private key _`userkey.pem`_ (password: hello) decrypt the file _`doc-encrypted.xml`_. Using the given public key _`pub-userkey.pem`_ check if its signature is correct. Upload the message inside the file using the template **exercise2.doc**:

| **Message** | **Signature correct?** |
| :---------- | :--------------------- |
| &nbsp;      | &nbsp;                 |

### Task 4

Create an XML file, sign and encrypt it using the private key from the **Task 2**. Test if you can decrypt it and verify the signature.
