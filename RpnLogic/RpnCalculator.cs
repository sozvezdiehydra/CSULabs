using RpnLogic.Tokens;

namespace RpnLogic
{

    public class RpnCalculator
    {
        public readonly double Result; // вот это поле

        public double inputX { get; set; } // вот это свойство
        
        private readonly List<FunctionProperties> functions;


        public RpnCalculator()
        {
            functions = new()
            {
                new FunctionProperties("cos", 1),
                new FunctionProperties("sin", 1),
                new FunctionProperties("log", 2),
                new FunctionProperties("tg", 1),
                new FunctionProperties("ln", 1),
                new FunctionProperties("lg", 1),
                new FunctionProperties("sqrt", 1),
                new FunctionProperties("Asqrt", 2),
                new FunctionProperties("exp", 1),
                new FunctionProperties("sign", 1),
                new FunctionProperties("Acos", 1),
                new FunctionProperties("Asin", 1),
                new FunctionProperties("Atg", 1)

            };
        }

        public RpnCalculator(string expression) : this() 
        {
            List<Token> rpn = ToRpn(Tokenize(expression));
            Result = Calculate(rpn);
        }
        public RpnCalculator(string expression, double varX) : this()
        {
            List<Token> rpn = ToRpn(Tokenize(expression)); 
            Result = CalculateWithX(rpn, varX);
        }
        private List<Token> Tokenize(string input)
        {
            List<Token> tokens = new List<Token>();
            string number = string.Empty;
            for(int i = 0; i < input.Length; i++)
            {
                char c = input[i]; 

                if (char.IsDigit(c))
                {
                    number += c; 
                }
                else if (c == ',' || c == '.')
                {
                    number += ",";
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^') 
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number)));
                        number = string.Empty;
                    }
                    tokens.Add(new Operation(c));
                }
                else if (c == '(' || c == ')')
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number)));
                        number = string.Empty;
                    }
                    tokens.Add(new Parenthesis(c));
                }
                else if (c == ';')
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number)));
                        number = string.Empty;
                    }
                    tokens.Add(new SeparateArgument());
                }
                else if (Number.CheckX(c))
                {
                    tokens.Add(new Number(c));
                }
                else if(functions.Any(f => f.Name.StartsWith(c)))
                { 
                    string rest = string.Empty; // rest - оставшаяся строка
                    for(int j = i; j < input.Length; j++) 
                    {
                        rest += input[j]; 
                    }

                    var function = functions.FirstOrDefault(f => rest.StartsWith(f.Name));
 
                    if(function != null) 
                    {
                        tokens.Add(new Function(function.Name, function.Arguments));
                        i += function.Name.Length - 1; 
                    }
                }
            }

            if (number != string.Empty)
            {
                tokens.Add(new Number(double.Parse(number)));
            }

            return tokens;
        }

        private static List<Token> ToRpn(List<Token> tokens)
        {
            List<Token> rpnOutput = new List<Token>();
            Stack<Token> operators = new Stack<Token>();
            string number = string.Empty;

            foreach (Token token in tokens)
            { 
                if (operators.Count == 0 && !(token is Number))
                {
                    operators.Push(token);
                    continue;
                }

                if (token is Operation) 
                {
                    if (operators.Peek() is Parenthesis)
                    {
                        operators.Push(token);
                        continue;
                    }

                    Operation first = (Operation)token;
                    Operation second = (Operation)operators.Peek();

                    if (first.Priority > second.Priority)
                    {
                        operators.Push(token);
                    }
                    else if (first.Priority <= second.Priority)
                    {
                        while (operators.Count > 0 && !(token is Parenthesis))
                        {
                            rpnOutput.Add(operators.Pop());
                        }
                        operators.Push(token);
                    }
                }
                else if (token is Parenthesis paranthesis)
                {
                    if (paranthesis.isClosing)
                    {
                        while (!(operators.Peek() is Parenthesis))
                        {
                            rpnOutput.Add(operators.Pop());
                        }

                        operators.Pop();

                        if (operators.Count > 0 && operators.Peek() is Function)
                        {
                            rpnOutput.Add(operators.Pop());
                        } 
                    }
                    else
                    {
                        operators.Push(paranthesis);
                    }
                }
                else if (token is Number num)
                {
                    rpnOutput.Add(num);
                }
                else if(token is Function function)
                { 
                    operators.Push(function);
                }
                else if(token is SeparateArgument arg)
                {
                    while (operators.Count > 0 && !(operators.Peek() is Parenthesis parenthesis && !parenthesis.isClosing))
                    {
                        rpnOutput.Add(operators.Pop()); 
                    }
                }
            }

            while (operators.Count > 0)
            {
                rpnOutput.Add(operators.Pop());
            }
            return rpnOutput;
        }
        private static double OperationExpress(double first, double second, char operation)
        {
            double result = 0;

            switch (operation)
            {
                case '+':
                    result = first + second;
                    break;
                case '-':
                    result = second - first;
                    break;
                case '*':
                    result = first * second;
                    break;
                case '/':
                    result = second / first;
                    break;
                case '^':
                    result = Math.Pow(second, first);
                    break;
            }

            return result;
        }

        public static double Calculate(List<Token> rpnTokens)
        {
            Stack<double> binCalculator = new Stack<double>();
            double result = 0;
            for (int i = 0; i < rpnTokens.Count; i++)
            {
                if (rpnTokens[i] is Number value)
                {
                    binCalculator.Push(value.Value);
                }
                else
                {
                    double firstNum = binCalculator.Pop();
                    double secondNum = binCalculator.Pop();

                    char oper = (char)(Operation)rpnTokens[i];
                    result = OperationExpress(firstNum, secondNum, oper);
                    binCalculator.Push(result);
                }
            }
            return binCalculator.Peek();
        }
        
        public static double CalculateWithX(List<Token> rpnCalc, double inputX)
        { 
            Stack<double> tempCalc = new Stack<double>();

            foreach (Token token in rpnCalc)
            {
                if (token is Number num)
                {
                    if (Number.CheckX(num.ValueX))
                    {
                        tempCalc.Push(inputX);
                    }
                    else
                    {
                        tempCalc.Push(num.Value);
                    }
                }
                else if (token is Operation)
                {
                    double first = tempCalc.Pop();
                    double second = tempCalc.Pop();
                    var op = (Operation)token;
                    double result = OperationExpress(first, second, op.Symbol);
                    tempCalc.Push(result);
                } 
                else if(token is Function function) 
                {
                    int argumentCount = function.Arguments;

                    var arguments = new List<double>();

                    for (int i = 0; i < argumentCount; i++)
                    {
                        arguments.Add(tempCalc.Pop());
                    }
                    arguments.Reverse();

                    double result;
                    switch (function.Name)
                    {
                        case "cos":
                            result = Math.Cos(arguments[0]); 
                            break;
                        case "sin":
                            result = Math.Sin(arguments[0]);
                            break;
                        case "log":
                            // тут сначала число, потом основание
                            result = Math.Log(arguments[0], arguments[1]);
                            break;
                        case "tg":
                            result = Math.Tan(arguments[0]);
                            break;
                        default:
                        case "ln":
                            result = Math.Log (arguments[0]);
                            break;
                        case "lg":
                            result = Math.Log10(arguments[0]);
                            break;
                        case "sqrt":
                            result = Math.Sqrt(arguments[0]);
                            break;
                        case "exp":
                            result = Math.Exp(arguments[0]);
                            break;
                        case "sign":
                            result = Math.Sign(arguments[0]);
                            break;
                        case "arccos":
                            result = Math.Acos(arguments[0]);
                            break;
                        case "arcsin":
                            result = Math.Asin(arguments[0]);
                            break;
                        case "arctg":
                            result = Math.Atan(arguments[0]);
                            break;
                        case "Asqrt":
                            result = Math.Pow(arguments[0], 1 / arguments[1]);
                            break;
                            throw new ArgumentException("No suitable function");
                    }
                    tempCalc.Push(result);
                }
            } 
            return tempCalc.Peek();
        }
    }
}