using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using HttpLib;

namespace Client2
{
    class Client2: TcpServer
    {
        protected override string ReceiveMessage(string message, string endpoint)
        {
            XDocument doc = XDocument.Parse(message);

            bool encrypted = doc.Descendants().Any(element =>
                element.Name.LocalName == "RequiresEncryption" &&
                element.Value.Trim().Equals("true", StringComparison.OrdinalIgnoreCase));

            if (encrypted)
            {
                Console.WriteLine("Client 2: encrypted update ignored.");
                return "";
            }

            var company = doc.XPathSelectElement("//*[1]/*[2]/*[1]/*[1]")!.Value;
            var price = doc.XPathSelectElement("//*[1]/*[2]/*[1]/*[2]")!.Value;

            Console.WriteLine("Client 2: incoming update: {0},{1}",
                company,
                price);
            return "";
        }

        static void Main( string[] args )
        {
            Client2 server = new Client2();
            Console.WriteLine( "Client2 started..." );
            server.Run( "127.0.0.1", 15002 );
        }
    }
}
