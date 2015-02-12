using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    public class Logger
    {
        public static void Log(string msg)
        {
            System.Diagnostics.Debug.WriteLine("[" + DateTime.Now + "][DEBUG]: " + msg);
        }
    }
}
