using System;
using System.Collections.Generic;
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
        public HandleRequests handleRequests;
        
        public Client(TcpClient cl, int count)
        {
            TcpClient = cl;
            Id = count;
            handleRequests = new HandleRequests();
        }

    }
}
