using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpnLogic.Tokens
{
    public class Number : Token
    {
        public double Value { get; }
        public char ValueX { get; }

        public Number(double value)
        {
            Value = value;
        }

        public Number(char valueX)
        {
            ValueX = valueX;
        }

        public static bool CheckX(char valueX)
        {
            return valueX is 'x' or 'X';
        }
    }
}
