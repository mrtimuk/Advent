using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.D11_Password
{
    class D11_Password
    {
        void Main()
        {
            var s = "hepxcrrq";

            while (!CheckAsc(s) || !CheckPairs(s))
                s = Incr(s);

            Console.WriteLine("New password {0}", s);
        }

        bool CheckAsc(string s)
        {
            var c = s[0];
            var l = 0;
            for (int i = 1; i < s.Length; i++)
            {
                if (s[i] == c+1) l++;
                else l = 1;

                c = s[i];
                if (l == 3) return true;
            }
            return false;
        }

        bool CheckPairs(string s)
        {
            var c = s[0];
            var p = 0;
            for (int i = 1; i < s.Length; i++)
            {
                if (c == s[i])
                {
                    p++;
                    if (p == 2) return true;
                    c = '@';
                }
                else c = s[i];
            }
            return false;
        }

        string Incr(string s)
        {
            var cs = s.ToCharArray();
            for (int i = s.Length - 1; i >= 0; i--) if (!Incr(ref cs[i])) break;
            return new string(cs);
        }

        bool Incr(ref char c)
        {
            c++;
            if (c == 'i' || c == 'o' || c == 'l') c++;
            if (c > 'z') { c = 'a'; return true; }
            return false;
        }
    }
}
