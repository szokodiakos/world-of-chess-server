using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    public class Turn
    {
        public Color WhichPlayer{ get; set; }
        public Figure WhichFigure { get; set; }
        public string FromFieldCode { get; set; }
        public string ToFieldCode { get; set; }

        public Turn()
        {
        }

        public override string ToString()
        {
            return "(Player=" + WhichPlayer + ", FigureType=" + WhichFigure.FigureType + ", FigureColor=" + WhichFigure.Color + ", FromField=" + FromFieldCode + ", ToField=" + ToFieldCode + ")";
        }
    }
}
