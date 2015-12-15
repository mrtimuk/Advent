using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Advent.D06_LightGrid
{
    class D6_LightGrid
    {
        public void Run()
        {
            var s = File.ReadAllText("D6.txt");

            var rCoords = new Regex(@"(\d+),(\d+) through (\d+),(\d+)");

            var l = new byte[1000 * 1000];
            foreach (var line in s.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var coords = rCoords.Match(line);
                var x1 = int.Parse(coords.Groups[1].Value);
                var y1 = int.Parse(coords.Groups[2].Value);
                var x2 = int.Parse(coords.Groups[3].Value);
                var y2 = int.Parse(coords.Groups[4].Value);

                if (line.StartsWith("turn on"))
                    for (int x = x1; x <= x2; x++)
                        for (int y = y1; y <= y2; y++)
                            l[x * 1000 + y] += 1;
                else if (line.StartsWith("turn off"))
                    for (int x = x1; x <= x2; x++)
                        for (int y = y1; y <= y2; y++)
                            l[x * 1000 + y] = (byte)Math.Max(0, l[x * 1000 + y] - 1);
                else if (line.StartsWith("toggle"))
                    for (int x = x1; x <= x2; x++)
                        for (int y = y1; y <= y2; y++)
                            l[x * 1000 + y] += 2;
            }
            var result = 0;
            for (int x = 0; x < 1000; x++)
                for (int y = 0; y < 1000; y++)
                    result += l[x * 1000 + y];

            Console.WriteLine(result);
        }
    }
}