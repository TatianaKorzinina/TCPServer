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
        private Boolean _isRunning;
        private int counter = 0;
        private List<Client> clients = new List<Client>();




        public TcpServer()
        {
            int port;
            FromFile("settings.txt").TryGetValue("port", out port);
            _server = new TcpListener(IPAddress.Any, port);
            _isRunning = true;
        }

        public void Start()
        {
            _server.Start();
            Console.WriteLine("server started");
            LoopClients();
        }



        public static Dictionary<string, int> FromFile(string fileName)
        {
            List<string> settingsList = new List<string>();
            using (var streamRead = new StreamReader(fileName))
            {
                while (!streamRead.EndOfStream)
                {
                    settingsList.Add(streamRead.ReadLine());
                }

                var set = settingsList.ToDictionary(x => x.Split(':')[0], x => Int32.Parse(x.Split(':')[1]));
                return set;
            }
        }

        public void LoopClients()
        {
            while (_isRunning)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();
                counter += 1;
                // client found.
                Client client = new Client(newClient, counter);
                clients.Add(client);

                // create a thread to handle communication
                Thread t = new Thread(client.HandleClient);
                t.Start();
            }
        }
    }
}
