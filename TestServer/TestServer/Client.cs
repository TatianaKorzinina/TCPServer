using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    class Client
    {
        public readonly int Id;
        public bool Report { get; set; }
        public bool Log { get; set; }
        public readonly TcpClient TcpClient;
        private readonly HandleRequests handleRequests;
        
        public Client(TcpClient cl, int count)
        {
            TcpClient = cl;
            Id = count;
            handleRequests = new HandleRequests();
        }

        public string Handle(string request)
        {
            return handleRequests.HandleRequest(this, request);
        }

        public void HandleClient()
        {
            // retrieve client from parameter passed to thread
            //Client client = (Client)obj;
            // sets two streams
            using (StreamWriter sWriter = new StreamWriter(TcpClient.GetStream(), Encoding.ASCII))
            using (StreamReader sReader = new StreamReader(TcpClient.GetStream(), Encoding.ASCII))
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

                        sWriter.WriteLine(Handle(sData));
                        sWriter.Flush();

                        watch.Stop();

                        if (Report)
                        {
                            sWriter.WriteLine("command " + sData + " compleded in " + watch.ElapsedMilliseconds +
                                              " ms");
                            sWriter.Flush();
                        }

                        if (Log)
                        {
                            using (StreamWriter stream =
                                new StreamWriter($"log of {Id} client.txt", true))
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

