using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    static class RequestProcessor
    {
        private static readonly IDictionary<string, Func<string,Client, string>> _commands;
        static RequestProcessor()
        {
            // Ininit commands dictionary
            _commands = new Dictionary<string, Func<string, Client, string>>();

            foreach (var methodInfo in typeof(RequestProcessor).GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                // Search methodInfo's with ClientCommand attribute
                if (methodInfo.GetCustomAttributes(typeof(ClientCommandAttribute), true).FirstOrDefault() is ClientCommandAttribute commandAttribute)
                {
                    _commands.Add(commandAttribute.Name, (parameter, client) => (string)methodInfo.Invoke(null, new object[]{parameter,client}));
                }
            }
        }


        public  static bool HandleRequest( Client client, string str, out string answer)
        {
            
            string parameter = null;
            // check if the command is empty

            bool success = str.Length != 0;

            // divide the incoming string into command and parameter
            string command = str.Split(':')[0];
            if (str.Contains(':'))
            {
                parameter = str.Split(':')[1];
            }

            // recognize the command and execute it
            if (_commands.ContainsKey(command))
            {
                answer = _commands[command].Invoke(parameter,client);
            }
            else
            {
                answer = "invalid command";
            }

            
            return success;
        }

        [ClientCommand("report")]
        private static string SwitchReport(string parameter, Client client)
        {
            string answer = null;
                // recognize the parameter and switch report mode
                switch (parameter)
                {
                    case "on":
                        client.Report = true;
                        break;
                    case "off":
                        client.Report = false;
                        break;
                    default:
                        answer = "invalid parameter";
                        break;
                }    
            return answer;
        }

        [ClientCommand("log")]
        private static string SwitchLogs(string parameter, Client client)
        {
            string answer = null;

            // recognize the parameter and switch log mode
            switch (parameter)
                {
                    case "on":
                        client.Log = true;
                        break;
                    case "off":
                        client.Log = false;
                        break;
                    default:
                        answer="invalid parameter";
                        break;
                } 
            return answer;
        }

        [ClientCommand("time")]
        private static string Time(string parameter, Client client)
        {
            return DateTime.Now.ToString();
        }
    }
}


