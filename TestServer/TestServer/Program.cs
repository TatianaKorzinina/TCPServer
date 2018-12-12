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

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("enter port number");
           var port =  Convert.ToInt32(Console.ReadLine());
            TcpServer server = new TcpServer(port);

        }
    }

    class TcpServer
    {
        private TcpListener _server;
        private Boolean _isRunning;
        private int counter = 0;
      
        private List<Client> clients = new List<Client>();

        public TcpServer(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();
            Console.WriteLine("server started");
            _isRunning = true;

            LoopClients();
        }

        public void LoopClients()
        {
            while (_isRunning)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();
                counter += 1;
                // client found.

                clients.Add(new Client(newClient,counter));
                
                // create a thread to handle communication
                Thread t = new Thread((HandleClient));
                t.Start(clients[clients.Count-1]);
            }
        }

        public void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            Client client = (Client)obj;

            

            // sets two streams
              StreamWriter sWriter = new StreamWriter(client.TcpClient.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.TcpClient.GetStream(), Encoding.ASCII);
          

            Boolean bClientConnected = true;
            String sData = null;

            while (bClientConnected)
            {
                try
                {



                    Stopwatch watch = new Stopwatch();
                    // reads from stream
                    sData = sReader.ReadLine();

                    watch.Start();

                    MessageReceived(sData, sWriter, client);

                    if (sData == "time")
                    {
                        var date = DateTime.Now;

                        sWriter.WriteLine(date);
                        sWriter.Flush();
                    }

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
                            new StreamWriter(string.Format("log of {0} client.txt", client.Number), true))
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

        // set log и report values
        public static void MessageReceived(string str, StreamWriter writer, Client client)
        {
            if (str == "report:on" || str == "report:off" || str == "log:on"
                || str == "log:off" || str == "time")
            {
                string option = null;
                string command = str.Split(':')[0];
                if (str.Contains(':'))
                {
                    option = str.Split(':')[1];
                }

                if (command == "report")
                {
                    switch (option)
                    {
                        case "on":
                            client.Report = true;
                            break;
                        case "off":
                            client.Report = false;
                            break;
                    }
                }

                if (command == "log")
                {
                    switch (option)
                    {
                        case "on":
                            client.Log = true;
                            break;
                        case "off":
                            client.Log = false;
                            break;
                    }
                }
            }
            else
            {
                
                writer.WriteLine("invalid command");
                writer.Flush();
                return;
                
            }
        }
    }
}
