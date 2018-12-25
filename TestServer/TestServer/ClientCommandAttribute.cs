using System;

namespace TestServer
{
    internal class ClientCommandAttribute : Attribute
    {
        public string Name;

        public ClientCommandAttribute(string name)
        {
            Name = name;
        }
    }
}