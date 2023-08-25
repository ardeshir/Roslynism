using System.Collections;
using System.Reflection;
using System.Transactions;
using System.IO;  
using RoslynCompiler;  


namespace Roslynism  {  
public class Program  { 
  
        static void Main(string[] args)  
        {  

                if (args.Length <= 1)  {  
                 Console.WriteLine("Please provide a .rl file as an argument.");  
                 return;  
                }  
  
            string code = File.ReadAllText(args[1]);  
  
            // Compile the code into an assembly  
            Assembly assembly = Compiler.Compile(code, "NewAssembly");  

            if (assembly != null)  {  
            // Create an instance of the class  
                    Type type = assembly.GetType("LinearAlgebraEquations.LinearEquationSolver");  
                    object instance = Activator.CreateInstance(type);  
                    // Invoke the method on the instance  
                    MethodInfo method = type.GetMethod("Solve");  

                    double result = (double)method.Invoke(instance, null);

                    Console.WriteLine(result);    // Output: 64.6796875 

            }   else {  
                Console.WriteLine("Compilation failed. Check your code for errors.");  
            }  

    }
  }
}