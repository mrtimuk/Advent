using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.D10_LookSay
{
    class D10_LookSay
    {
        public void Run()
        {
            var s = "1113222113";
            for (int i = 0; i < 50; i++)
                s = LookSay(s);

            Console.WriteLine("Final length: {0}", s.Length);
        }

        string LookSay(string s)
        {
            var res = new StringBuilder(4000000);
            for (int i = 0; i < s.Length; )
            {
                char c = s[i];
                int j = 0;
                while (i < s.Length && s[i] == c) { j++; i++; };

                res.Append(j);
                res.Append(c);
            }
            return res.ToString();
        }
    }
}