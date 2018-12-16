using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    class RequestProcessor
    {
        
        public  bool HandleRequest( Client client, string str, out string answer)
        {
            
            string parameter = null;
            // check if the command is empty

            bool success = !(str.Length == 0);

            // divide the incoming string into command and parameter
            string command = str.Split(':')[0];
            if (str.Contains(':'))
            {
                parameter = str.Split(':')[1];
            }
            // recognize the command and execute it
            switch (command)
            {
                    case "report":
                        answer = SwitchReport(parameter, client);
                        break;
                    case "log":
                        answer = SwitchLogs(parameter, client);
                        break;
                    case "time":
                        answer = Time();
                        break;
                    default:
                        answer ="invalid command";
                        break;
            }

            return success;
        }

        private string SwitchReport(string parameter, Client client)
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

        private string SwitchLogs(string parameter, Client client)
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

        private string Time()
        {
            return DateTime.Now.ToString();
        }
    }
    
}


