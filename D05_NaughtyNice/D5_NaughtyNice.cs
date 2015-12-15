using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent.D05_NaughtyNice
{
    class D5_NaughtyNice
    {
        public void Run()
        {
            var s = File.ReadAllText("D5.txt");

            var r1 = new Regex("[aeiou]");
            var r2 = new Regex("(.)\\1");
            var r3 = new Regex("ab|cd|pq|xy");

            var r4 = new Regex("(..).*\\1");
            var r5 = new Regex("(.).\\1");

            var a = 0;
            foreach (var line in s.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                //        if (r3.Match(line).Success) {
                //            Console.WriteLine("bad couplets");
                //            continue;
                //        }
                //       
                //        if (r1.Matches(line).Count < 3){
                //            Console.WriteLine("not enough vowels");
                //            continue;
                //        }
                //       
                //        if (!r2.Match(line).Success) {
                //            Console.WriteLine("no double letters");
                //            continue;
                //        }
                if (r4.Matches(line).Count < 1)
                {
                    Console.WriteLine("no double repeat");
                    continue;
                }

                if (r5.Matches(line).Count < 1)
                {
                    Console.WriteLine("no 3 char palindrome");
                    continue;
                }
                a++;
            }
            Console.WriteLine("{0}", a);
        }
    }
}