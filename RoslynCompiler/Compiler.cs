using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;

namespace RoslynCompiler {

public class Compiler
{
    public static Assembly Compile(string sourceCode, string assemblyName)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);  
        string systemRuntimePath = Path.Combine(assemblyPath, "System.Runtime.dll");  
        string systemCorePath = Path.Combine(assemblyPath, "System.Core.dll");  
  
        MetadataReference[] references = new MetadataReference[]  
        {  
            MetadataReference.CreateFromFile(systemRuntimePath),  
            MetadataReference.CreateFromFile(systemCorePath),  
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),  
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),  
        };  



        CSharpCompilation compilation = CSharpCompilation.Create(assemblyName)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(references)
            .AddSyntaxTrees(syntaxTree);

        using (MemoryStream ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                foreach (Diagnostic error in result.Diagnostics)
                {
                    Console.WriteLine($"{error.Id}: {error.GetMessage()}");
                }

                return null;
            }

            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly = Assembly.Load(ms.ToArray());
            return assembly;
        }
    }
}
}