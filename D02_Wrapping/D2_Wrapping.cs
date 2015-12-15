using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Advent.D02
{
    class D2_Wrapping
    {
        public void Run()
        {
            string s = File.ReadAllText("D2.txt");
            var reader = new StringReader(s);
            var re = new Regex(@"(\d+)x(\d+)x(\d+)");

            var total = 0;
            var ribbon = 0;
            do
            {
                var line = reader.ReadLine();
                if (line == null) break;

                var match = re.Match(line);
                var sides = new[] { 
                    int.Parse(match.Groups[1].Value), 
                    int.Parse(match.Groups[2].Value), 
                    int.Parse(match.Groups[3].Value) };
                sides = sides.OrderBy(v => v).ToArray();
                var a1 = sides[0] * sides[1];
                var a2 = sides[1] * sides[2];
                var a3 = sides[2] * sides[0];
                var subtotal = 2 * a1 + 2 * a2 + 2 * a3 + Math.Min(a1, Math.Min(a2, a3));
                total += subtotal;

                ribbon += 2 * sides[0] + 2 * sides[1] + sides[0] * sides[1] * sides[2];
            }
            while (true);

            Console.WriteLine("Total: " + total);
            Console.WriteLine("Ribbon: " + ribbon);
        }
    }
}
