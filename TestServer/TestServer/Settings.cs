using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{   [CollectionDataContract]
    class Settings
    {   [DataMember]
        public int Port { get; set; }
    }
}
