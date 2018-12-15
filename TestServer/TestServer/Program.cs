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
            int port;
            TcpServer.FromFile("settings.txt").TryGetValue("port", out port);
            TcpServer server = new TcpServer(port);
            server.Start();
        }
    }

    //class TcpServer
    
        
}
