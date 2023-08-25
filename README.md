### Here is an example C# code that uses the Roslyn Compiler API to read in a C# file, compile it into a DLL and load its types to use in another assembly:

```csharp
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.IO;
using System.Reflection;

namespace RoslynCompiler {

public class Compiler
{
    public static Assembly Compile(string sourceCode, string assemblyName)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        MetadataReference[] references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")),
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
```

You can use this class as follows:

```csharp
string code = @"
using System;

namespace MyNamespace
{
    public class MyClass
    {
        public static void SayHello()
        {
            Console.WriteLine(""Hello from Roslyn!"");
        }
    }
}
";

Assembly assembly = RoslynCompiler.Compile(code, "MyAssembly");

Type type = assembly.GetType("MyNamespace.MyClass");
MethodInfo method = type.GetMethod("SayHello");
method.Invoke(null, null); // outputs "Hello from Roslyn!" to the console

```

## This code dynamically compiles the `code` string into a DLL named "MyAssembly" and loads it into memory using the `Assembly.Load` method. You can then use reflection to get a `MethodInfo` object for the `SayHello` method and invoke it to execute the code.

## To add Microsoft Roslyn compiler package to your project, you may use the following `dotnet add package` statements:
- If you want to add the packages to a .NET Core run:

```
dotnet add package Microsoft.CodeAnalysis
dotnet add package Microsoft.CodeAnalysis.CSharp
```