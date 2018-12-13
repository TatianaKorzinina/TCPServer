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
                 FromFile("settings.json").TryGetValue("port", out port);
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
            using (var stream = File.OpenRead(fileName))
            {

                var serializer = new DataContractJsonSerializer(typeof(Dictionary<string,int>),
                    new DataContractJsonSerializerSettings() { UseSimpleDictionaryFormat = true });
                    // ConsoleLogger.WriteMessage($"Loaded configuration from {stream.Name}", MessageType.Info);
                return serializer.ReadObject(stream) as Dictionary<string,int>;
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

            clients.Add(new Client(newClient, counter));
                
            // create a thread to handle communication
            Thread t = new Thread((HandleClient));
            t.Start(clients[clients.Count - 1]);
        }
    }

    public void HandleClient(object obj)
    {
        // retrieve client from parameter passed to thread
        Client client = (Client)obj;
            // sets two streams
        using (StreamWriter sWriter = new StreamWriter(client.TcpClient.GetStream(), Encoding.ASCII))
        using (StreamReader sReader = new StreamReader(client.TcpClient.GetStream(), Encoding.ASCII))
            {            
                Boolean bClientConnected = true;
                String sData = null;

                while (bClientConnected)
                {
                    try
                    {
                        Stopwatch watch = new Stopwatch();
                        // reads from stream
                        sData = sReader.ReadLine();

                        if (sData.Length == 0)
                        {
                            continue;
                        }

                        watch.Start();

                        sWriter.WriteLine(client.handleRequests.HandleRequest(sData, client));
                        sWriter.Flush();

                        watch.Stop();

                        if (client.Report)
                        {
                            sWriter.WriteLine("command " + sData + " compleded in " + watch.ElapsedMilliseconds +
                                              " ms");
                            sWriter.Flush();
                        }

                        if (client.Log)
                        {
                            using (StreamWriter stream =
                                new StreamWriter($"log of {client.Id} client.txt", true))
                            {
                                stream.WriteLine(sData);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        break;
                    }
                }
            }
        }
    }
}
