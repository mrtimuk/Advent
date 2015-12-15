using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEscape
{
    class D08_Escape
    {
        public void Run()
        {
            var input = File.ReadAllLines("D08.txt");

            var total = 0;
            foreach (var line in input)
            {
                var strlen = Escape(line);
                total += line.Length - strlen;
            }
            Console.WriteLine("Total {0}", total);
        }

        static int Unescape(string line)
        {
            var strlen = 0;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (i == 0 && c == '"') continue;
                if (i == line.Length - 1 && c == '"') continue;

                if (c == '\\')
                {
                    i++;
                    c = line[i];
                    if (c == 'x') i += 2;
                }
                strlen++;
            }
            return strlen;
        }

        static int Escape(string line)
        {
            var strlen = 2;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (c == '\\' || c == '"')
                    strlen++;

                strlen++;
            }
            return strlen;
        }
    }
}
