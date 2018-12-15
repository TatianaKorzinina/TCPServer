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
            FromFile("settings.txt").TryGetValue("port", out port);
            TcpServer server = new TcpServer(port);
            server.Start();

             Dictionary<string, int> FromFile(string fileName)
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
        }
    }

    //class TcpServer
    
        
}
