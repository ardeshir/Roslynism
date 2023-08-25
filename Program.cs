using System.Collections;
using System.Reflection;
using System.Transactions;
using RoslynCompiler;  

namespace Roslynism  {  
public class Program  { 
  
        static void Main(string[] args)  
        {  
string code = @"
using System;  
  
namespace LinearAlgebraEquations  {  

    public class LinearEquationSolver {  

        public static double SolveEquation(params object[] equation)  
        {  
            double result = Convert.ToDouble(equation[0]);  
            char currentOperator = '+';  
  
            for (int i = 1; i < equation.Length; i++)  
            {  
                var current = equation[i];  
  
                if (current is char op)  
                {  
                    currentOperator = op;  
                }  
                else if (current is Func<double, double> func)  
                {  
                    result = ApplyFunction(result, func);  
                }  
                else if (current is double operand)  
                {  
                    result = ApplyOperator(result, currentOperator, operand);  
                }  
                else  
                {  
                    throw new ArgumentException(""Invalid equation element: "" + current);  
                }  
            }  
  
            return result;  
        }  
  
        private static double ApplyOperator(double operand1, char op, double operand2)  
        {  
            switch (op)  
            {  
                case '+':  
                    return operand1 + operand2;  
                case '-':  
                    return operand1 - operand2;  
                case '*':  
                    return operand1 * operand2;  
                case '/':  
                    return operand1 / operand2;  
                default:  
                    throw new ArgumentException(""Invalid operator: "" + op);  
            }  
        }  
  
        private static double ApplyFunction(double value, Func<double, double> func)  
        {  
            return func(value);  
        }  
 
        static double DoubleValue(double x) => x * 2;  
        static double HalfValue(double x) => x / 2;  
        static double SquareValue(double x) => x * x;  
        static double CubeValue(double x) => x * x * x;  
        static double IncrementValue(double x) => x + 1;  
        static double DecrementValue(double x) => x - 1; 

       public double Solve() {  
            double result = LinearEquationSolver.SolveEquation( 
            2.0, '+', new Func<double, double>(DoubleValue),  
            3.0, '*', new Func<double, double>(HalfValue), 
            4.0, '/', DoubleValue(2.0), 
            '-', new Func<double, double>(SquareValue), 
            5.0, '+', new Func<double, double>(CubeValue), 
            6.0, '*', new Func<double, double>(IncrementValue), 
            '/', DecrementValue(7.0)
            );

            return result; 
        }  
    }  
} 

";  
  
    // Compile the code into an assembly  
     Assembly assembly = Compiler.Compile(code, "NewAssembly");  

     if (assembly != null)  {  
      // Create an instance of the class  
        Type type = assembly.GetType("LinearAlgebraEquations.LinearEquationSolver");  
        object instance = Activator.CreateInstance(type);  
        // Invoke the method on the instance  
        MethodInfo method = type.GetMethod("Solve");  
        // Object[] input = {2.0, '+', new Func<double, double>(DoubleValue),  3.0, '*', new Func<double, double>(HalfValue), 4.0, '/', new Func<double, double>(DoubleValue), '-', new Func<double, double>(SquareValue), 5.0, '+', new Func<double, double>(CubeValue), 6.0, '*', new Func<double, double>(IncrementValue), '/', new Func<double, double>(DecrementValue)};  

          double result = (double)method.Invoke(instance, null);

            Console.WriteLine(result);    // Output: 64.6796875 

        }   else {  
            Console.WriteLine("Compilation failed. Check your code for errors.");  
        }  

    }
  }
}