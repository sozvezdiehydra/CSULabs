using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace RpnLogic
{
    class RpnConsole
    {
        class RPN
        {
            static string GetInput()
            {
                Console.Write("Write your expression: ");
                return Console.ReadLine();
            }
            static string GetInputX()
            {
                Console.Write("Write your X: ");
                return Console.ReadLine();
            }
            static void Main(string[] args)
            {
                string input = GetInput();
                int inputX = int.Parse(GetInputX());
                RpnCalculator rpn = new RpnCalculator(input, inputX);
                double result = rpn.Result;

                // Console.WriteLine($"Ваше выражение в ОПЗ: {string.Join(" ", toRPN(tokens))}"); //// to do normal output
                Console.WriteLine($"Result: {result}");

            }
        }
    }
    
}
