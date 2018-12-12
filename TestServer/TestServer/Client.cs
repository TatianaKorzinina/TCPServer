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
        public int Number { get; set; }
        public bool Report { get; set; }
        public bool Log { get; set; }
        public readonly TcpClient TcpClient;

        public Client(TcpClient cl, int count)
        {
            TcpClient = cl;
            Number = count;
            Log = false;
            Report = false;
        }

    }
}
