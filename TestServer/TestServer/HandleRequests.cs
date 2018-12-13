using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    class HandleRequests
    {
        private  Dictionary<string, List<string>> requestsWithParams = new Dictionary<string, List<string>>();
        private  List<string> requestsWithoutParams = new List<string>();
        private  List<string> options= new List<string>();
        public HandleRequests()
        {   options.Add("on");
            options.Add("off");
            requestsWithoutParams.Add("time");
            requestsWithParams.Add("log",options);
            requestsWithParams.Add("report", options);
        }
        
        public  string HandleRequest( string str, Client client)
        {

            List<string> args = new List<string>();
            string ansver = null;
                string option = null;
                string command = str.Split(':')[0];
                if (str.Contains(':'))
                {
                    option = str.Split(':')[1];
                }

                requestsWithParams.TryGetValue(command, out args);
                
            if (option!= null && requestsWithParams.ContainsKey(command) && args.Contains(option))
            {
                if (command == "report")
                {
                   ansver = SwitchReport(option, client);  
                }

                if (command == "log")
                {
                    ansver = SwitchLogs(option, client);
                    
                }
            }
            else if (option == null && requestsWithoutParams.Contains(command))
            {
                if (command == "time")
                {
                   ansver = Time();
                }
            }
            else
            {
                ansver = "invalid command";
            }
            return ansver;
        }

        public string SwitchReport(string option, Client client)
        {
            switch (option)
            {
                case "on":
                    client.Report = true;
                    break;
                case "off":
                    client.Report = false;
                    break;
            }
            return null;
        }

        public string SwitchLogs(string option, Client client)
        {
            switch (option)
            {
                case "on":
                    client.Log = true;
                    break;
                case "off":
                    client.Log = false;
                    break;
            }
            return null;
        }

        public string Time()
        {
            return DateTime.Now.ToString();
        }
    }
    
}


