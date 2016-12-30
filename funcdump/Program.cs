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
            Parser.Start(filepath(args)); //Start parser
            string[] funcList = Parser.FunctionList();
            Console.WriteLine("Functions:");
            int i = 1;
            foreach(string func in funcList)
            {
                Console.WriteLine("[" + i.ToString() + "] " +func);
                i++;
            }
            while (true)
            {
                Console.Write("Search: ");
                var searchTerms = Console.ReadLine();
                if(searchTerms == "[:!/--]")
                {
                    break;
                }
                var searchResults = arrContains(funcList, searchTerms);
                Console.Clear();
                Console.WriteLine("Results:");
                i = 1;
                foreach (string func in searchResults)
                {
                    Console.WriteLine("[" + (whichLine(func, funcList) + 1).ToString() + "] " + func);
                    i++;
                }
                Console.WriteLine("Type [:!/--] (brackets included) to exit search and dump functions.");
            }
            Console.Write("Enter line ID: ");
            var id = int.Parse(Console.ReadLine());
            var specifiedLine = funcList[id - 1];
            Console.WriteLine("[DEBUG] You specified this line: " + specifiedLine);
            var dumped_code = Parser.dump(specifiedLine);
            Console.Clear();
            foreach (string code in dumped_code)
            {
                Console.WriteLine(code);
            }
            Console.ReadKey();

        }

        static int whichLine(string line, string[] array)
        {
            for (int i = 0; i <= array.Length - 1; i++)
            {
                if (array[i] == line)
                {
                    return i;
                }
            }
            return 0;
        }

        static string[] arrContains(string[] array, string containment)
        {
            List<string> results = new List<string>();
            foreach (string line in array)
            {
                if (Contains(line, containment))
                {
                    results.Add(line);
                }
            }
            return results.ToArray();
        }

        static bool Contains(string container, string containment)
        {
            if (container.IndexOf(containment) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
