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
       // private  Dictionary<string, List<string>> requestsWithParams = new Dictionary<string, List<string>>();
        private  List<string> requestsWithoutParams = new List<string>();
       // private  List<string> options= new List<string>();
        public HandleRequests()
        {   //options.Add("on");
            //options.Add("off");
            requestsWithoutParams.Add("time");
            requestsWithoutParams.Add("log");
            requestsWithoutParams.Add("report");
        }
        
        public  string HandleRequest( Client client, string str)
        {

            List<string> args = new List<string>();
            string answer = null;
                string parameter = null;
                string command = str.Split(':')[0];
                if (str.Contains(':'))
                {
                    parameter = str.Split(':')[1];
                }

            //requestsWithParams.TryGetValue(command, out args);

            try
            {
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
                    default: throw new Exception("invalid command");

                }
            }
            catch (Exception e)
            {
                answer = e.Message;
            }
               
            return answer;
        }

        public string SwitchReport(string parameter, Client client)
        {
            string answer = null;
            try
            {
                switch (parameter)
                {
                    case "on":
                        client.Report = true;
                        break;
                    case "off":
                        client.Report = false;
                        break;
                    default:
                        throw new Exception("invalid parameter");
                }

            }
            catch (Exception e)
            {
                answer = e.Message;
                //throw;
            }

            return answer;
        }

        public string SwitchLogs(string parameter, Client client)
        {
            string answer = null;
            try
            {
                switch (parameter)
                {
                    case "on":
                        client.Log = true;
                        break;
                    case "off":
                        client.Log = false;
                        break;
                    default:
                        throw new  Exception("invalid parameter exception");
                }

            }
            catch (Exception e)
            {
                answer = e.Message;
                //throw;
            }

            return answer;
        }

        public string Time()
        {
            return DateTime.Now.ToString();
        }
    }
    
}


