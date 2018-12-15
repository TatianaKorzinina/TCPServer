using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace TestServer
{
    class TcpServer
    {
        private readonly TcpListener _server;
        private int counter = 0;
        private List<Client> clients = new List<Client>();

        public TcpServer(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            _server.Start();
            Console.WriteLine("server started");
            LoopClients();
        }

        public void LoopClients()
        {
            while (true)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();
                counter += 1;
                // client found.
                Client client = new Client(newClient, counter);
                clients.Add(client);

                // create a thread to handle communication
                Task t = new Task(client.HandleClient);
                t.Start();
            }
        }
    }
}
