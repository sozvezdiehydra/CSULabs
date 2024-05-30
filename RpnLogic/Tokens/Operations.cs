using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpnLogic.Tokens
{
    class Operation(char symbol) : Token
    {
        public char Symbol { get; } = symbol;
        public int Priority { get; } = GetPriority(symbol);

        private static int GetPriority(char symbol)
        {
            switch (symbol)
            {
                case '(': return 0;
                case ')': return 0;
                case '+': return 1;
                case '-': return 1;
                case '*': return 2;
                case '/': return 2;
                case '^': return 3;
                default: return 4;
            }
        }

        public static explicit operator char(Operation v)
        {
            throw new NotImplementedException();
        }
    }

    class Parenthesis(char symbol) : Token
    {
        public bool isClosing { get; set; } = symbol == ')';


    }
}
