using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using HttpLib;

namespace Client1
{
    class Client1: TcpServer
    {
        private const string SharedKey = "sse-tutorial-04";

        protected override string ReceiveMessage(string message, string endpoint)
        {
            XDocument doc = XDocument.Parse(message);
            string company;
            string price;

            bool encrypted = doc.Descendants().Any(element =>
                element.Name.LocalName == "RequiresEncryption" &&
                element.Value.Trim().Equals("true", StringComparison.OrdinalIgnoreCase));

            if (encrypted)
            {
                var encryptedPayload = doc.Descendants().FirstOrDefault(element => element.Name.LocalName == "EncryptedPayload")?.Value
                    ?? throw new FormatException("Encrypted payload missing.");

                var decryptedXml = Encryption.Decrypt(encryptedPayload, SharedKey);
                var decryptedDoc = XDocument.Parse(decryptedXml);
                company = decryptedDoc.XPathSelectElement("//*[1]/*[1]")!.Value;
                price = decryptedDoc.XPathSelectElement("//*[1]/*[2]")!.Value;
            }
            else
            {
                company = doc.XPathSelectElement("//*[1]/*[2]/*[1]/*[1]")!.Value;
                price = doc.XPathSelectElement("//*[1]/*[2]/*[1]/*[2]")!.Value;
            }
            Console.WriteLine("Client 1: incoming {2}update: {0},{1}",
                company,
                price,
                encrypted ? "encrypted " : "");
            return "";
        }

        static void Main(string[] args)
        {
            Client1 server = new Client1();
            Console.WriteLine("Client1 started...");
            server.Run("127.0.0.1", 15001);
        }
    }

}
