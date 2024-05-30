using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpnLogic.Tokens
{
    class Function(string name, int arguments) : Token
    {
        public string Name { get; init; } = name;
        public int Arguments { get; init; } = arguments;
    }
    record class FunctionProperties(string Name, int Arguments);

    class SeparateArgument : Token
    {
    }
}
