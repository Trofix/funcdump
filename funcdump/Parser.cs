using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
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

        public static string[] FunctionList()
        {
            string funcline = "";
            List<String> results = new List<String>();
            for (int i = 0; i <= filelines.Length - 1; i++)
            {
                funcline = "";
                if (Contains(filelines[i], "{"))
                {
                    if (Regex.IsMatch(filelines[i], "^\\s+{\\s*$"))
                    {
                        
                        if (!Contains(filelines[i-1], "else"))
                        {
                            funcline = filelines[i - 1];
                        }
                    }
                    else
                    {
                        funcline = filelines[i];
                    }
                    //Console.WriteLine(Regex.IsMatch(filelines[i], "\\s{"));
                    results.Add(funcline);
                }
            }
            return tabCut(Blacklister(results.ToArray()));
        }

        private static string[] Blacklister(string[] lines)
        {
            bool fail = false;
            string[] blacklistEntries = { "if ", "if(", "for ", "for(", "foreach ","foreach(", "{", "class ", "//" };
            List<String> results = new List<String>();
            for (int i = 0; i <= lines.Length - 1; i++)
            {
                fail = false;
                /*if (!Contains(line, "if") || !Contains(line, "for") || !Contains(line, "foreach") || !Contains(line, "{") || !Contains(line, "class"))
                {
                    results.Add(line);
                }*/
                foreach (string entry in blacklistEntries)
                {
                    if (Contains(lines[i], entry))
                    {
                        fail = true;
                        break;
                    }
                }

                if (lines[i] == "")
                {
                    fail = true;
                }

                if (fail != true)
                {
                    results.Add(lines[i]);
                }
            }
            return results.ToArray();
        }

        private static string[] tabCut(string[] lines)
        {
            List<String> results = new List<string>();
            foreach(string line in lines)
            {
                bool startCapture = false;
                string newline = "";
                foreach(char character in line.ToCharArray())
                {
                    if (Regex.IsMatch(character.ToString(), "\\w") || startCapture)
                    {
                        if (!startCapture)
                        {
                            startCapture = true;
                        }
                        newline += character.ToString();
                    }
                }
                results.Add(newline);
            }
            return results.ToArray();
        }

        static int whichLineContains(string line, string[] array)
        {
            for (int i = 0; i <= array.Length - 1; i++)
            {
                if (Contains(array[i], line))
                {
                    return i;
                }
            }
            return 0;
        }

        private static string[] getWholeFunc(string line)
        {
            List<string> results = new List<string>();
            int i = whichLineContains(line, filelines);
            string mainline = filelines[i];
            results.Add(mainline);
            for (int x = 1; i + x <= filelines.Length - 1; x++)
            {
                string currLine = filelines[i + x];
                results.Add(currLine);
                if (Regex.IsMatch(currLine, "\\s*}"))
                {
                    break;
                }
            }
            return results.ToArray();
        }

        public static string[] dump(string line)
        {
            List<string> func_calls = new List<string>();
            var func = getWholeFunc(line).ToList();
            foreach(string command in func)
            {
                if (Regex.IsMatch(command, "\\s*.+\\(.*\\);"))
                {
                    func_calls.Add(command);
                }
            }
            return func_calls.ToArray();
        }

        private static bool Contains(string container, string containment)
        {
            if (container.IndexOf(containment) >= 0)
            {
                return true;
            } else {
                return false;
            }
        }

        public static string[] Purifier(string[] functions)
        {
            
            List<String> results = new List<String>();
            foreach (string function in functions)
            {
                int funcNameStart = 0;
                int funcNameEnd = 0;
                int symCount = 0;
                int lastSpace = 0;
                bool inString = false;
                for (int i = 0; i <= function.Length - 1; i++)
                {
                    if (function.ToCharArray()[i] == '\'')
                    {
                        if (function.ToCharArray()[i-1] == '\\')
                        {
                            if (function.ToCharArray()[i - 2] == '\\')
                            {
                                inString = !inString;
                            }
                        }else
                        {
                            inString = !inString;
                        }
                    }

                    if (function.ToCharArray()[i] == '\"')
                    {
                        if (function.ToCharArray()[i - 1] == '\\')
                        {
                            if (function.ToCharArray()[i - 2] == '\\')
                            {
                                inString = !inString;
                            }
                        }
                        else
                        {
                            inString = !inString;
                        }
                    }

                    if (function.ToCharArray()[i] == ' ')
                    {
                        lastSpace = i;
                    }

                    if(function.ToCharArray()[i] == '(')
                    {
                        if (!inString)
                        {
                            symCount++;
                        }
                        if (symCount == 1)
                        {
                            funcNameStart = lastSpace + 1;
                        }
                    }

                    if (function.ToCharArray()[i] == ')')
                    {
                        if (!inString)
                        {
                            symCount--;
                            if (symCount == 0)
                            {
                                funcNameEnd = i;
                                break;
                            }
                        }
                    }
                }
                results.Add(function.Substring(funcNameStart, funcNameEnd - funcNameStart));
            }
            return results.ToArray();
        }
    }
}
