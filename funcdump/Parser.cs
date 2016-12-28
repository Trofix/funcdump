using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace funcdump
{
    class Parser
    {
        private static string[] filelines;
        private static string file;

        public static void Start(string _file)
        {
            file = _file;
            filelines = File.ReadAllLines(file);
            
        }

        private static string[] SupArrSearch(string[] array, string searchTerm) //SUPARRSEARCH!(get it?!)
        {
            List<String> results = new List<String>();
            foreach (string line in array)
            {
                if (line.IndexOf(searchTerm) != 0)
                {
                    results.Add(line);
                }
            }
            return results.ToArray();
        }
    }
}
