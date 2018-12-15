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
            bool success = !(str.Length == 0);
   
            string command = str.Split(':')[0];
            if (str.Contains(':'))
            {
                parameter = str.Split(':')[1];
            }
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


