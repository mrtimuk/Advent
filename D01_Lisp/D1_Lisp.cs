using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.D01
{
    class D1_Lisp
    {
        public void Run()
        {
            string s = File.ReadAllText("D1.txt");

            int j = 0;
            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case '(': j++; break;
                    case ')': j--; break;
                }
                if (j == -1)
                {
                    Console.WriteLine("Index" + i+1);
                    break;
                }
            }
        }
    }
}
