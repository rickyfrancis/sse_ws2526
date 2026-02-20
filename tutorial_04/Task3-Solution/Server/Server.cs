using HttpLib;
using System.Xml.Linq;

namespace Server
{
    class Server: TcpServer
    {
        private const string BROKER_URL = "http://127.0.0.1:14000/";
        private const string EncryptionHeader = "<sec:RequiresEncryption xmlns:sec=\"http://example.org/security\">true</sec:RequiresEncryption>";
        private const string SharedKey = "sse-tutorial-04";

        protected override string ReceiveMessage(string message, string endpoint)
        {
            Console.WriteLine("Server: incoming message: " + message);
            return "";
        }

        static void Main(string[] args)
        {
            Server server = new Server();
            server.sendStockUpdate("FB", "35.5");
            server.sendStockUpdate("TW", "30.0", true);
            server.sendStockUpdate("LI", "15.0", true);
            server.Run("127.0.0.1", 13000);
        }


        private void sendStockUpdate(string company, string price,bool encrypted = false)
        {
            HttpLib.HttpClient client = new HttpLib.HttpClient();
            int tries = 0;
            bool clientAcknowledged;
            do
            {
                var encryptionHeader = encrypted ? EncryptionHeader : "";
                string content = string.Format(
                    "<env:Envelope xmlns:env=\"http://www.w3.org/2001/09/soap-envelope\">" +
                        "<env:Header>" +
                            "<n:StatusRequest xmlns:n=\"http://example.org/status\" env:mustUnderstand=\"true\"/>" +
                            "{2}" +
                       "</env:Header><env:Body>" +
                        createSOAPBody(company, price, encrypted) +
                    "</env:Body></env:Envelope>",
                    company, price, encryptionHeader);
                var response = client.Post(BROKER_URL, "application/soap+xml", content);               
                clientAcknowledged = CheckIfClientAcknowledgedReciept(response.Content);
                if (!clientAcknowledged)
                    Console.WriteLine("Server: broker is busy. repeating transmission...");
                tries++;
            } while (tries < 15 && !clientAcknowledged);
        }

        private string createSOAPBody( string company, string price, bool encrypted )
        {
            string body = 
                          string.Format( "<ex:StockUpdate xmlns:ex=\"http://example.org\">" +
                          "<ex:Company>{0}</ex:Company>" +
                          "<ex:Price>{1}</ex:Price>" +
                          "</ex:StockUpdate>", company , price);

            if (encrypted)
            {
                var encryptedBody = Encryption.Encrypt(body, SharedKey);
                return string.Format("<sec:EncryptedPayload xmlns:sec=\"http://example.org/security\">{0}</sec:EncryptedPayload>", encryptedBody);
            }

            return body;
        }

        private bool CheckIfClientAcknowledgedReciept(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return false;

            try
            {
                var document = XDocument.Parse(response);
                var state = document.Descendants().FirstOrDefault(element => element.Name.LocalName == "State")?.Value;
                if (string.IsNullOrWhiteSpace(state))
                    return false;

                state = state.Trim().ToUpperInvariant();
                return state == "RECIEVED" || state == "RECEIVED";
            }
            catch
            {
                return false;
            }
        }


    }
}
