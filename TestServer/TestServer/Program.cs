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
            string port;    
            // load port number from file
            LoadSettings("settings.txt").TryGetValue("port", out port);
            int portNumber = Int32.Parse(port);
            TcpServer server = new TcpServer(portNumber);
            server.Start();            
        }

        static Dictionary<string, string>LoadSettings(string fileName)
        {
            List<string> settingsList = new List<string>();
            // try to load settings from file
            try
            {
                using (var streamRead = new StreamReader(fileName))
                {
                    while (!streamRead.EndOfStream)
                    {
                        settingsList.Add(streamRead.ReadLine());
                    }

                    var set = settingsList.ToDictionary(x => x.Split(':')[0], x => x.Split(':')[1]);
                    return set;
                }
            }
            // set default port number if the file doesn't exist
            catch (FileNotFoundException)
            {
                Dictionary<string, string> defaultSettings = new Dictionary<string, string>
                {
                    { "port", "5555" }
                };
                return defaultSettings;
            }

        }
    }        
}
