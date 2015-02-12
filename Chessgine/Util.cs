using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    public class Util
    {
        public static Color GetOppositeColor(Color color)
        {
            return color == Color.WHITE ? Color.BLACK : Color.WHITE;
        }
    }
}
