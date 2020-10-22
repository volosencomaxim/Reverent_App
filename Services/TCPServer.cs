using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Reverent_App.Services
{
    class TCPServer
    {
        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader sr;
        private StreamWriter sw;
        private SearchService _searchService;
        public TCPServer()
        {
            listener = new TcpListener(System.Net.IPAddress.Any, 1302);
            _searchService = new SearchService();
        }

        public void StartServer(JArray data)
        {
            listener.Start();
            while (true)
            {
                Console.WriteLine("Server is ready. Waiting for a connection.");
                //Console.WriteLine(data);
                client = listener.AcceptTcpClient();
                Console.WriteLine("Client accepted.");
                stream = client.GetStream();
                sr = new StreamReader(client.GetStream());
                sw = new StreamWriter(client.GetStream());
                try
                {
                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);
                    int recv = 0;
                    foreach (byte b in buffer)
                    {
                        if (b != 0)
                        {
                            recv++;
                        }
                    }
                    string request = Encoding.UTF8.GetString(buffer, 0, recv);

                    var dataString = _searchService.CommandController(data, request);
  
                    sw.WriteLine(dataString);
                    sw.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong.");
                    sw.WriteLine(e.ToString());
                }
            }
        }
    }
}
