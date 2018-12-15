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



        public static Dictionary<string, int> FromFile(string fileName)
        {
            List<string> settingsList = new List<string>();
            
            try
            {
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
            catch (FileNotFoundException)
            {
                Dictionary<string, int> defaultSettings = new Dictionary<string, int>();
                defaultSettings.Add("port", 5555);
                return defaultSettings;
            }

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
                Thread t = new Thread(client.HandleClient);
                t.Start();
            }
        }
    }
}
