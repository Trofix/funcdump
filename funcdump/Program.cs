using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using funcdump;

namespace funcdump
{
    class Program
    {
        static string path;
        static string[] filelines;

        static void Main(string[] args)
        {
            Parser.Start("D:\\Projects\\funcdump\\funcdump\\Parser.cs"/*filepath(args)*/); //Start parser
            string[] funcList = Parser.FunctionList();
            string[] pureFuncList = Parser.Purifier(Parser.FunctionList());
            Console.WriteLine("FUNCLIST:");
            foreach(string func in funcList)
            {
                Console.WriteLine(func);
            }
            Console.WriteLine("PUREFUNCLIST:");
            foreach (string func in funcList)
            {
                Console.WriteLine(func);
            }
            Console.ReadKey();
        }

        static string filepath(string[] args)
        {
            if (args.Length == 0) //If no args...
            {
                Console.Write("Enter file path: "); //...then ask for file path.
                return Console.ReadLine(); //Load file path into path variable, since we can't load into args[] duh
            }
            else //If file path was specified in args...
            {
                return args[0]; //..then load file path into path variable.
            }
        }
    }
}
