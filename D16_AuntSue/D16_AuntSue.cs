using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.D16_AuntSue
{
    class D16_AuntSue
    {
        public void Run()
        {
            var lines = File.ReadAllLines(@"D16_AuntSue\D16.txt");

            var Detected = new Dictionary<string, Func<int,bool>>
            {
                {"children", i => i == 3},
                {"cats", i => i > 7},
                {"samoyeds", i => i == 2},
                {"pomeranians", i => i < 3},
                {"akitas", i => i == 0},
                {"vizslas", i => i == 0},
                {"goldfish", i => i < 5},
                {"trees", i => i > 3},
                {"cars", i => i == 2},
                {"perfumes", i => i == 1}
            };
            
            var regex = new Regex(@"(\w+): (\d+)");
            var j = 0;
            foreach (var items in lines.Select(line => regex
                                             .Matches(line)
                                             .Cast<Match>()
                                             .ToDictionary(m => m.Groups[1].Value, m => int.Parse(m.Groups[2].Value))))
            {
                j++;
                if (items.All(item => Detected[item.Key](item.Value)))
                    Console.WriteLine("Sue {0}", j);
            }
        }
    }
}