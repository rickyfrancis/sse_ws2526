namespace SSE
{
    public class Middleware
    {
        private Func<object, string> _serverCallback;
        private Task _serverTask;
        private CancellationTokenSource _ts;

        private object Demarshall(string line)
        {
            // --------------------- Implement ---------------------
            // TODO: Deserialize the parameter
            // --------------------- /Implement -------------------
            return "TODO-DEMARSHALLED-DATA";
        }


        private string Marshall(object input)
        {
            // --------------------- Implement ---------------------    
            // TODO: Serialize the parameter         
            // --------------------- /Implement ---------------------
            return "TODO-MARSHALLED-DATA";
        }

        public async Task<string> SendObjectTo(string address, object input)
        {
            string ip = address.Split(':')[0];
            int port = int.Parse(address.Split(':')[1]);

            string payload = Marshall(input);

            Console.WriteLine($"\tMiddleware: Transferring payload '{payload}' to {ip}:{port}");

            string answer = await TcpRequest.Do(ip, port, payload);
            answer = answer.TrimEnd('\0'); // deleting all \0 of buffer for printing
            return answer;
        }


        public async Task<string> CallFunction(string address, string name, string[] args)
        {
            // --------------------- Implement ---------------------
            // TODO: Contact server and pass procedure call
            // --------------------- /Implement -------------------            
            string answer = "";
            return answer;
        }

        protected virtual string ProcessIncomingRequest(string line)
        {
            line = line.TrimEnd('\0'); // deleting all \0 of buffer for printing
            Console.WriteLine($"\tMiddleware: Received payload '{line}'");
            object answer = Demarshall(line);
            return _serverCallback(answer);
        }

        public void StartServer(string ip, int port, Func<object, string> callback)
        {
            _serverCallback = callback;
            _ts = new CancellationTokenSource();
            _serverTask = TcpServer.Start(ip, port, _ts, ProcessIncomingRequest);
        }

        public void StopServer()
        {
            _ts.Cancel();
            _serverTask.Wait();
        }
    }
}
