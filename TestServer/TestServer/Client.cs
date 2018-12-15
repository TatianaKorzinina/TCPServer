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
        private readonly RequestProcessor requestProcessor;
        
        public Client(TcpClient cl, int count)
        {
            TcpClient = cl;
            Id = count;
            requestProcessor = new RequestProcessor();
        }

        public string Handle(string request,out bool isEmpty)
        {
            return requestProcessor.HandleRequest(this, request, out isEmpty);
        }

        public void HandleClient()
        {
            // sets two streams
            using (StreamWriter sWriter = new StreamWriter(TcpClient.GetStream(), Encoding.ASCII))
            using (StreamReader sReader = new StreamReader(TcpClient.GetStream(), Encoding.ASCII))
            {
                String data = null;

                while (true)
                {
                    try
                    {
                        Stopwatch watch = new Stopwatch();
                        // reads from stream
                        data = sReader.ReadLine();
                        bool isEmpty;
                        var answer = Handle(data, out isEmpty);
                        if (!isEmpty)
                        {
                            watch.Start();

                            sWriter.WriteLine(answer);
                            sWriter.Flush();

                            watch.Stop();

                            if (Report)
                            {
                                sWriter.WriteLine("command " + data + " completed in " + watch.ElapsedMilliseconds +
                                                  " ms");
                                sWriter.Flush();
                            }

                            if (Log)
                            {
                                using (StreamWriter stream =
                                    new StreamWriter($"log of {Id} client.txt", true))
                                {
                                    stream.WriteLine(data);
                                }
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

