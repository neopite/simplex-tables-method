using System;

namespace ConsoleApp1
{
    class Program
    {
        private static double[,] table =
        {
            {4,-4, 3},
            {4,1, 2},
            {6,3, 1},
            {0,-3, -5}
        };
        
        
        static void Main(string[] args)
        {
            SimplexMethod simplexMethod = new SimplexMethod(table);
            double[] answer = simplexMethod.CalculateSimplexTable();
            double maxFunctionValue = 3 * answer[1] + 5 * answer[0];
            Console.WriteLine("X2 : " + answer[0]);
            Console.WriteLine("X1 : " + answer[1]);
            Console.WriteLine("Answer is  : " +maxFunctionValue);
        }
    }
}
