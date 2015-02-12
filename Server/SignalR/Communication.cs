using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.SignalR
{
    public class Communication
    {
        public IDisposable SignalR { get; set; }
        const string ServerURI = "http://localhost:8080";
        public bool ServerOnline { get; set; }
        public Communication()
        {
            ServerOnline = false;
        }
    }
}
