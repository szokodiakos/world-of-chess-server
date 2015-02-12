using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    public class GameArgs : EventArgs
    {
        public string Message { get; set; }
        public string Extra { get; set; }

        public GameArgs(string message, string extra)
        {
            this.Message = message;
            this.Extra = extra;
        }
    }
}
